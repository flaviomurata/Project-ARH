using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    CharacterController _characterController;
    Animator _animator;
    PlayerInput _playerInput;

    int _isWalkingHash;
    int _isRunningHash;

    Vector2 _currentMovementInput;
    Vector2 _currentMovementInput;
    Vector2 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;

    float _rotationFactorPerFrame = 15.0f;
    float _runMultiplier = 4.0f;
    int _zero = 0;

    float _gravity  = -9.8f;
    float _groundedGravity = -.05f;

    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 4.0f;
    float _maxJumpTime = .75f;
    bool _isJumping = false;
    int _isJumpingHash;
    int _jumpCountHash;
    bool _isJumpAnimating = false;
    int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;

    PlayerBaseState _currentState;

    void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

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
