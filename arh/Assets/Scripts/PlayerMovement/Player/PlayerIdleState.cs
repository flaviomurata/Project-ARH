using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void enterState(PlayerStateManager player)
    {
        player.rb.velocity = Vector2.zero;
    }

    public override void updateState(PlayerStateManager player)
    {

    }
}
