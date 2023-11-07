using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weighted pool of room data objects. 
/// The probability that a given room data is pulled is 
/// its assigned weight, divided by the sum of weights of all items in the pool.
/// </summary>
[CreateAssetMenu(menuName = "Office/Room Data")]
public class RoomData : ScriptableObject
{
    [Header("Hover over fields for tooltips :)")]

    [Tooltip("Prefab corresponding to the room's contents. This will be instantiated when the room is entered.")]
    public GameObject roomPrefab;

    [Header("Allowed door directions")]
    [Tooltip("A list of directions where doors can be placed (North/East/South/West). " +
    "Defaults to all directions, but if the provided room should have fewer doors, " +
    "remove them from the list to prevent rooms from being generated in places without doors." +
    "Note that on room generation, there may be fewer doors than specified, depending on how map gen goes " +
    "(or yell at Allen to fix this if it becomes a problem)." +
    "Should only include vectors corresponding to the four cardinal directions.")]
    public List<Vector2Int> doorLocations = new List<Vector2Int>(new Vector2Int[]{
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    });

    // P.S. I'm allowing a room to be generated with fewer doors than specified since it 
    // allows us to make fewer rooms than necessary.
}