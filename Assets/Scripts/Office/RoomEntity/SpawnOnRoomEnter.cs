using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnOnRoomEnter : RoomActivatable
{
    public List<GameObject> enemyList = new List<GameObject>();
    [Header("Prefab for the thing to spawn when the room is entered for the first time.")]
    public GameObject spawningPrefab;

    public override void OnRoomFirstEnter()
    {
        // Spawns an enemy as a child of the spawner object
        enemyList.Add(Instantiate(spawningPrefab, transform.position, Quaternion.identity, transform));
    }

    public override bool IsRoomCleared()
    {
        enemyList = enemyList.Where(enemy => enemy != null).ToList();
        return enemyList.Count == 0;
    }
}