
using UnityEngine;

public class MultiplicativeStatItem : BaseItem
{
    [Header("Just set this to whatever variables are being used by the player.")]
    public PlayerVariables playerVars;

    [Header("Stat changes:")]
    public float attackDamageMultiplier = 1.0f;

    public float attackCooldownMultiplier = 1.0f;

    public override void OnPickUp(PlayerManager player)
    {
        playerVars.attackCooldown *= attackCooldownMultiplier;
        playerVars.attackDamage *= attackDamageMultiplier;
    }
}