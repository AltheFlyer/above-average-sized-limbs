using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVariables", menuName = "ScriptableObjects/Game Variables", order = 1)]
public class PlayerVariables : ScriptableObject
{
    //Player Movement
    public float playerSpeed;
    public int maxSpeed;
    public int dashSpeed;
    public float dashDuration; //how long the player will dash
    public float dashCooldown;  //cooldown period between dashes.

    // Player Health & Attack
    public int maxHealth;
    public float attackDamage;
    public float attackCooldown;
    //Combo
    public float comboTimeLimit;

    public void CopyInto(PlayerVariables other)
    {
        other.playerSpeed = playerSpeed;
        other.maxSpeed = maxSpeed;
        other.dashSpeed = dashSpeed;
        other.dashDuration = dashDuration;
        other.dashCooldown = dashCooldown;
        other.maxHealth = maxHealth;
        other.attackDamage = attackDamage;
        other.attackCooldown = attackCooldown;
        other.comboTimeLimit = comboTimeLimit;
    }
}
