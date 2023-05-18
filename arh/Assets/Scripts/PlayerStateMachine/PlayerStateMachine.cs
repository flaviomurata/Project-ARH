using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // state variables
    private PlayerBaseState _currentstate;
    private PlayerStateFactory _states;
    
    // getters and setters
    public PlayerBaseState CurrentState
    {
        get { return _currentstate; }
        set { _currentstate = value; }
    }
    
    public bool IsJumping
    {
        get { return _isJumping; }
    }
    
    private Collision _coll;
    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    
    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool _groundTouch;
    private bool _hasDashed;
    private bool _isJumping;

    public int side = 1;

    void Awake()
    {
        _states = new PlayerStateFactory(this);
        _currentstate = _states.Grounded();
        _currentstate.EnterState();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveIt();
    }

    void MoveIt()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumping = context.ReadValueAsButton();
    }
}
