using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void enterState(PlayerStateManager player);

    public abstract void updateState(PlayerStateManager player);
}
