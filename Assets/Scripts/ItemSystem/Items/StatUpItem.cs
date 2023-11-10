
using UnityEngine;

public class StatUpItem : BaseItem
{
    [Header("Just set this to whatever game constants are being used by the player.")]
    public GameConstants gameConstants;

    [Header("Stat changes:")]
    public float deltaPlayerSpeed;
    public int deltaMaxSpeed;
    public int deltaDashSpeed;
    public int deltaHealth;
    public int deltaMaxHealth;

    public override void OnPickUp(PlayerManager player)
    {
        gameConstants.playerSpeed += deltaPlayerSpeed;
        gameConstants.maxSpeed += deltaMaxSpeed;
        gameConstants.dashSpeed += deltaDashSpeed;
        gameConstants.maxHealth += deltaMaxHealth;

        // Well, turns out health is a bit more painful to set, 
        // as multiple things seem to track it right now.
        // TODO: update health and max health properly.
    }
}