using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    PlayerIdleState idleState = new PlayerIdleState();
    PlayerMovingState movingState = new PlayerMovingState();

    [SerializeField] public InputActionReference movement, jump;

    public Rigidbody2D rb;

    private void OnEnable()
    {
        movement.action.performed += move;
        movement.action.canceled += stopMoving;
    }

    private void OnDisable()
    {
        movement.action.performed -= move;
        movement.action.canceled -= stopMoving;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = idleState;
        currentState.enterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.updateState(this);
    }

    private void move(InputAction.CallbackContext obj)
    {
        currentState = movingState;
        currentState.enterState(this);
    }

    private void stopMoving(InputAction.CallbackContext obj)
    {
        currentState = idleState;
        currentState.enterState(this);
    }
}
