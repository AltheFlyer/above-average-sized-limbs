using UnityEngine;

[CreateAssetMenu(fileName =  "GameConstants", menuName =  "ScriptableObjects/GameConstants", order =  1)]
public  class GameConstants : ScriptableObject
{
    //Player
    public float playerSpeed;
    public int maxSpeed;
    public int dashSpeed;
    public int maxHealth;

    //Enemy
    public int enemySpeed;
}
