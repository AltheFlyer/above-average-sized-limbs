
using UnityEngine;

public class StatUpItem : BaseItem
{
    [Header("Just set this to whatever variables are being used by the player.")]
    public PlayerVariables playerVars;

    [Header("Stat changes:")]
    public float deltaPlayerSpeed;

    public float deltaAttacksPerSecond;

    public float deltaAttackDamage;

    public int deltaDashSpeed;
    public int deltaHealth;
    public int deltaMaxHealth;

    private PlayerManager player;

    public void StatUp(PlayerManager player)
    {
        playerVars.UpdateRawSpeed(deltaPlayerSpeed);
        playerVars.UpdateInverseAttackCooldown(deltaAttacksPerSecond);
        playerVars.dashSpeed += deltaDashSpeed;
        playerVars.attackDamage += deltaAttackDamage;

        // Not great, we have to tell the damageable component that the health was changed too...
        playerVars.maxHealth += deltaMaxHealth;
        player.GetComponent<Damageable>()?.SetMaxHP(playerVars.maxHealth);
        player.GetComponent<Damageable>()?.Heal(deltaHealth);

        GlobalEventHandle.instance.playerStatChange.Raise(null);
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