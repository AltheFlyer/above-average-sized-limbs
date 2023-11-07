using UnityEngine;

/// <summary>
/// A weighted pool of room data objects. 
/// The probability that a given room data is pulled is 
/// its assigned weight, divided by the sum of weights of all items in the pool.
/// </summary>
[CreateAssetMenu(menuName = "Office/Room Data")]
public class RoomData : ScriptableObject
{
    [Tooltip("Prefab corresponding to the room's contents. This will be instantiated when the room is entered.")]
    public GameObject roomPrefab;
}