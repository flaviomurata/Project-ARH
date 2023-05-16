using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 value = new Vector2();
    // Start is called before the first frame update
    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(InputAction.CallbackContext context)
    {
        value = context.ReadValue<Vector2>();
    }
    
    public void jump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * Time.fixedDeltaTime * 5800f);
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(value.x, 0, 0) * (Time.fixedDeltaTime * 800f) );
    }
}
