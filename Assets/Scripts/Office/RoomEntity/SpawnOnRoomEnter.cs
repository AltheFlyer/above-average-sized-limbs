using UnityEngine;

public class SpawnOnRoomEnter : RoomActivatable
{
    [Header("Prefab for the thing to spawn when the room is entered for the first time.")]
    public GameObject spawningPrefab;

    public override void OnRoomFirstEnter()
    {
        Instantiate(spawningPrefab, transform.position, Quaternion.identity, transform);
    }
}