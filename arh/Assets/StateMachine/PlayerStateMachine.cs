using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerInput _playerInput;

    private int _isWalkingHash;
    private int _isRunningHash;

    private Vector2 _currentMovementInput;
    private Vector2 _currentMovement;
    private Vector2 _appliedMovement;
    private bool _isMovementPressed;
    private bool _isRunPressed;

    private float _rotationFactorPerFrame = 15.0f;
    private float _runMultiplier = 4.0f;
    private int _zero = 0;

    private float _gravity  = -9.8f;
    private float _groundedGravity = -.05f;

    private bool _isJumpPressed = false;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 4.0f;
    private float _maxJumpTime = .75f;
    private bool _isJumping = false;
    private int _isJumpingHash;
    private int _jumpCountHash;
    private bool _requireNewJumpPress = false;
    private int _jumpCount = 0;
    private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    private Coroutine _currentJumpResetRoutine = null;

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    public PlayerBaseState CurrentState { get{ return _currentState; } set { _currentState = value; }}
    public Animator Animator { get { return _animator; }}
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; }}
    public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; }}
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; }}
    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; }}
    public int IsWalkingHash { get { return _isWalkingHash; }}
    public int IsRunningHash { get { return _isRunningHash; }}
    public int IsJumpingHash { get { return _isJumpingHash; }}
    public int JumpCountHash { get { return _jumpCountHash; }}
    public bool IsMovementPressed { get { return _isMovementPressed; }}
    public bool IsRunPressed { get {return _isRunPressed; }}
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; }}
    public bool IsJumping { set { _isJumping = value; }}
    public bool IsJumpPressed { get { return _isJumpPressed; }}
    public float GroundedGravity { get { return _groundedGravity; }}
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; }}
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; }}
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; }}
    public float RunMultiplier { get { return _runMultiplier; }}
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; }}

    void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;
        _playerInput.CharacterControls.Jump.started += OnJump;
        -PlayerInput.CharacterControls.Jump.canceled == OnJump;

        SetupJumpVariables();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        _currentState.UpdateStates();
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;

        _initialJumpVelocities.Add(1, _initialJumpVelocity);

        _jumpGravities.Add(0, _gravity);
        _jumpGravities.Add(1, _gravity);
    }

    void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}
