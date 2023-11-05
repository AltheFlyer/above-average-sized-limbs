using UnityEngine;

[CreateAssetMenu(menuName = "Office/Room Data")]
public class RoomData : ScriptableObject
{
    [Tooltip("Prefab corresponding to the room's contents. This will be instantiated when the room is entered.")]
    public GameObject roomPrefab;
}