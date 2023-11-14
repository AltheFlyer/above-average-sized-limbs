
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

    public override void OnPickUp(PlayerManager player)
    {
        playerVars.playerSpeed += deltaPlayerSpeed;
        playerVars.maxSpeed += deltaMaxSpeed;
        playerVars.dashSpeed += deltaDashSpeed;
        playerVars.maxHealth += deltaMaxHealth;

        // Well, turns out health is a bit more painful to set, 
        // as multiple things seem to track it right now.
        // TODO: update health and max health properly.
    }
}