
using UnityEngine;

public class MachineGunHands : BaseItem
{
    [Header("Just set this to whatever variables are being used by the player.")]
    public PlayerVariables playerVars;

    // Instead of thinking about attack cooldown (time per attack), 
    // think about the invers (attacks per unit time).
    // Try to linearly scale attacks per unit time, which shoudl result in a 
    // reciprocal relation.
    // e.g., a value of 0.5 means each stack of the item increases the number of attacks/second by 50%
    public float extraAttackRatioPerStack;

    private PlayerManager player;

    public void StatUp(PlayerManager player)
    {
        playerVars.UpdateInverseAttackCooldown(extraAttackRatioPerStack);
    }

    public override void OnPickUp(PlayerManager player)
    {
        this.player = player;

        StatUp(player);
    }

    public override void Stack()
    {
        base.Stack();

        StatUp(player);
    }
}