using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        _ctx.AppliedMovementX = _ctx.CurrentMovementInput.x * _ctx.RunMultiplier;
    }

    public override void ExitState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {
        if(!_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Idle());
        }

        else if(_ctx.IsMovementPressed && !_ctx.IsRunPressed)
        {
            SwitchState(_factory.Walk());
        }
    }
}
