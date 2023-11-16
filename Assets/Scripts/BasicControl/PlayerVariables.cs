using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVariables", menuName = "ScriptableObjects/Game Variables", order = 1)]
public class PlayerVariables : ScriptableObject
{
    //Player
    public float playerSpeed;
    public int maxSpeed;
    public int dashSpeed;
    public int maxHealth;

    public void CopyInto(PlayerVariables other)
    {
        other.playerSpeed = playerSpeed;
        other.maxSpeed = maxSpeed;
        other.dashSpeed = dashSpeed;
        other.maxHealth = maxHealth;
    }
}