using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        _ctx.JumpCount = 0;
    }

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsJumpingHash, false);
        if(_ctx.IsJumpPressed)
        {
            _ctx.RequireNewJumpPress = true;
        }
        _ctx.CurrentJumpResetRoutine = _ctx.StartCoroutine(IJumpResetRoutine());
        if(_ctx.JumpCount == 3)
        {
            _ctx.JumpCount = 0;
            _ctx.Animator.SetInteger(_ctx.JumpCountHash, _ctx.JumpCount);
        }
    }

    public override void InitializeSubState()
    {
        if(!_ctx.IsMovementPressed && !_ctx.IsRunPressed)
        {
            SetSubState(_factory.Idle());
        }

        else if(_ctx.IsMovementPressed && !_ctx.IsRunPressed)
        {
            SetSubState(_factory.Walk());
        }
        
        else{
            SetSubState(_factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
        if(_ctx.CharacterController.isGrounded)
        {
            SwitchState(_factory.Grounded());
        }
    }

    void HandleJump()
    {
        if(_ctx.JumpCount < 3 && _ctx.CurrentJumpResetRoutine != null)
        {
            StopCoroutine(_ctx.CurrentJumpResetRoutine);
        }

        _ctx.Animator.SetBool(_ctx.IsJumpingHash, true);
        _ctx.IsJumpAnimating = true;
        _ctx.IsJumping = true;
        _ctx.Jumpcount += 1;
        _ctx.Animator.SetInteger(_ctx.JumpCountHash, _ctx.JumpCount);
        _ctx.CurrentMovement.y = _ctx.InitialJumpVelocities[_ctx.JumpCount];
        _ctx.AppliedMovement.y = _ctx.InitialJumpVelocities[_ctx.JumpCount];
    }

    void HandleGravity()
    {
        bool isFalling = _ctx.CurrentMovementY <= 0.0f || !_ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if(isFalling)
        {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.JumpGravities[_ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * .5f, -20.0f);
        }

        else
        {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY = _ctx.CurrentMovementY + (_ctx.JumpGravities[_ctx.JumpCount] * Time.deltaTime);
            _ctx.AppliedMovementY = (previousYVelocity + _ctx.CurrentMovementY) * .5f;
        }
    }
}
