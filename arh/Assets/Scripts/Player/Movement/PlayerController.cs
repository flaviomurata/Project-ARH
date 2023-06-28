using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collision _coll;
    private SpriteRenderer _sr;
    
    private Vector2 _dir;
    private int _side = 1;
    
    [Space]
    [Header("Booleans")]
    public bool canJump;

    [Space]
    [Header("Stats")]
    public float movementSpeed = 10;
    public float jumpForce = 50;
    private float _movementInputDirection;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collision>();
        _sr = GetComponentInChildren<SpriteRenderer>();
    }
    
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        Walk(_dir);
    }

    private void CheckMovementDirection()
    {
        if (_movementInputDirection < 0)
        {
            _side = -1;
            Flip(_side);
        }
        else if (_movementInputDirection > 0)
        {
            _side = 1;
            Flip(_side);
        }
    }
    
    private void CheckInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _movementInputDirection = Input.GetAxisRaw("Horizontal");
            
        _dir = new Vector2(x, y);
        
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void CheckIfCanJump()
    {
        if (_coll.onGround && _rb.velocity.y < 0.1f)
        {
            canJump = true;
        }
        else canJump = false;
        }

    private void Jump()
    {
        if (canJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += Vector2.up * jumpForce;
        }
    }

    private void Walk(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * movementSpeed, _rb.velocity.y);
    }

    private void Flip(int side)
    {
        bool state = (side != 1);
        _sr.flipX = state;
    }
}
