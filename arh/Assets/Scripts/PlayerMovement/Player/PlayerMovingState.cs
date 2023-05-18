using UnityEngine;

public class PlayerMovingState : PlayerBaseState
{
    public override void enterState(PlayerStateManager player)
    {
        //setar animacao
        player.rb.velocity = new Vector2(10 * player.movement.action.ReadValue<float>(), player.rb.velocity.y);
        Debug.Log("aaa");
    }

    public override void updateState(PlayerStateManager player)
    {
        player.rb.velocity = new Vector2(10 * player.movement.action.ReadValue<float>(), player.rb.velocity.y);
        Debug.Log("aaa");
    }
}
