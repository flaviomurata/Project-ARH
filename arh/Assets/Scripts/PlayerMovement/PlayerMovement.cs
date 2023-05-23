using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("rb")] [SerializeField]
    private Rigidbody2D _rb;

    [FormerlySerializedAs("groundCheck")] [SerializeField]
    private Transform _groundCheck;

    [FormerlySerializedAs("groundLayer")] [SerializeField]
    private LayerMask _groundLayer;

    private GroundHandler IsGrounded;
    private bool _doubleJump;
    private float _horizontal;
    private float _speed = 2f;
    private float _jumpPower = 8f;

    private void OnEnable()
    {
        IsGrounded.IsGroundedEvent.AddListener(Jump);            
    }

    private void OnDisable()
    {
        IsGrounded.IsGroundedEvent.RemoveListener(Jump);
    }
    //todo: Assistir ao video de como fazer uma state machine com a movimentacao nova da unity
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * _speed, _rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_doubleJump && context.canceled)
        {
            _doubleJump = false;
        }

        if (context.performed)
        {
            if (_doubleJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
                _doubleJump = !_doubleJump;
            }
        }

        if (context.canceled && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
    }

    private void CanJump()
    {
        Jump();
    }
    
}