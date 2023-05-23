using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory)
    {
        _isRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _ctx.CurrentMovementY = _ctx.GroundedGravity;
        _ctx.AppliedMovementY = _ctx.GroundedGravity;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {

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
        if(_ctx.IsJumpPressed && !_ctx.RequireNewJumpPress)
        {
            SwitchState(_factory.Jump());
        }
    }
}
