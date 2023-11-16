
using UnityEngine;

public class StatUpItem : BaseItem
{
    [Header("Just set this to whatever variables are being used by the player.")]
    public PlayerVariables playerVars;

    [Header("Stat changes:")]
    public float deltaPlayerSpeed;
    public int deltaMaxSpeed;
    public int deltaDashSpeed;
    public int deltaHealth;
    public int deltaMaxHealth;

    public float deltaAttackCooldown;

    public override void OnPickUp(PlayerManager player)
    {
        playerVars.playerSpeed += deltaPlayerSpeed;
        playerVars.maxSpeed += deltaMaxSpeed;
        playerVars.dashSpeed += deltaDashSpeed;

        playerVars.attackCooldown += deltaAttackCooldown;

        // Not great, we have to tell the damageable component that the health was changed too...
        playerVars.maxHealth += deltaMaxHealth;
        player.GetComponent<Damageable>()?.SetMaxHP(playerVars.maxHealth);
        player.GetComponent<Damageable>()?.Heal(deltaHealth);

        GlobalEventHandle.instance.playerStatChange.Raise(null);
    }
}