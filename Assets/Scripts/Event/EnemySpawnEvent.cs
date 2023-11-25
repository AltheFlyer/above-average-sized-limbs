using UnityEngine;

[CreateAssetMenu(menuName = "Event/EnemySpawn")]
public class EnemySpawnEvent : GameEvent<EnemySpawnData>
{

}

public class EnemySpawnData
{
    public GameObject enemy;
    public Vector3 spawnLocation;

    public EnemySpawnData(GameObject enemy)
    {
        this.enemy = enemy;
        this.spawnLocation = enemy.transform.position;
    }
}
