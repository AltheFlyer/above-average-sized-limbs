using UnityEngine;

[CreateAssetMenu(menuName = "Office/Room Data")]
public class RoomData : ScriptableObject
{
    [Tooltip("Prefab corresponding to the room's contents. This will be instantiated when the room is entered.")]
    public GameObject roomPrefab;


    // WIP values!
    public Vector2[] doors = new Vector2[]{
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0),
    };
}