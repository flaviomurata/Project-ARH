using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private int _counter = 0;
    private float _horizontal;
    private float _speed = 2f;
    private float _jumpPower = 8f;


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
        if (context.performed)
        {
            if (IsGrounded() || _counter < 2)
            {
                _counter++;
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
            }
        }

        if (context.canceled && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        _counter = 0;
        return Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
    }
}