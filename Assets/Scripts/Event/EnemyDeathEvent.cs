using UnityEngine;

[CreateAssetMenu(menuName = "Event/EnemyDeath")]
public class EnemyDeathEvent : GameEvent<EnemyDeathData>
{

}

public class EnemyDeathData
{
    public GameObject enemy;
    public Vector3 deathLocation;

    public EnemyDeathData(GameObject enemy)
    {
        this.enemy = enemy;
        this.deathLocation = enemy.transform.position;
    }
}
