using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVariables", menuName = "ScriptableObjects/Game Variables", order = 1)]
public class PlayerVariables : ScriptableObject
{
    //Player Movement
    public float playerSpeed;
    public int maxSpeed;
    public int dashSpeed;

    // To limit speed scaling, we store the 
    // speed including modifications from items, 
    // and calculate the real player speed.
    private float rawSpeed;
    private float basePlayerSpeed;
    private float maxSpeedPlayerSpeedRatio;
    public float speedCap; // Maximum allowed real speed value

    public float dashDuration; //how long the player will dash
    public float dashCooldown;  //cooldown period between dashes.

    // Player Health & Attack
    public int maxHealth;
    public float attackDamage;
    public float attackCooldown;
    // Internals for attack cooldown math
    private float attacksPerSecond;
    private float baseAttackCooldown;


    //Combo
    public float comboTimeLimit;


    public void InitInternalStats()
    {
        rawSpeed = playerSpeed;
        basePlayerSpeed = playerSpeed;
        maxSpeedPlayerSpeedRatio = maxSpeed / playerSpeed;

        attacksPerSecond = 1 / attackCooldown;
        baseAttackCooldown = attackCooldown;
    }

    public void CopyInto(PlayerVariables other)
    {
        other.playerSpeed = playerSpeed;
        other.maxSpeed = maxSpeed;
        other.speedCap = speedCap;
        other.dashSpeed = dashSpeed;
        other.dashDuration = dashDuration;
        other.dashCooldown = dashCooldown;
        other.maxHealth = maxHealth;
        other.attackDamage = attackDamage;
        other.attackCooldown = attackCooldown;
        other.comboTimeLimit = comboTimeLimit;
    }

    /// <summary>
    /// Updates the real attack cooldown, given an update to the 
    /// inverse attack cooldown
    /// </summary>
    public void UpdateInverseAttackCooldown(float deltaAttacksPerSecond)
    {
        attacksPerSecond += deltaAttacksPerSecond;

        attackCooldown = 1 / attacksPerSecond;
    }

    public void UpdateRawSpeed(float deltaRawSpeed)
    {
        rawSpeed += deltaRawSpeed;
        // To prevent player speed from getting out of hand, we scale with with 
        // the hyperbolic tangent function.
        // This makes scaling roughly linear for small speed increases, 
        // while giving diminishing returns up to the specified speed cap.
        playerSpeed = basePlayerSpeed +
            (speedCap - basePlayerSpeed) * (float)System.Math.Tanh((rawSpeed - basePlayerSpeed) / (speedCap - basePlayerSpeed));
        maxSpeed = (int)(playerSpeed * maxSpeedPlayerSpeedRatio);
    }
}
