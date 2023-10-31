using UnityEngine;

[CreateAssetMenu(menuName = "Office/Room")]
public class Room : ScriptableObject
{
    [Tooltip("Prefab corresponding to the room's contents. This will be instantiated when the room is entered.")]
    public GameObject roomPrefab;
}