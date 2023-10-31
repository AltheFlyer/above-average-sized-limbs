using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName = "Office/Office Generator")]
public class OfficeGenerator : ScriptableObject
{

    public int numRooms;

    [Header("Special, predefined rooms")]
    // For now, the special rooms are fixed for simplicity, 
    // but feel free to change the type to RoomPool(s) to allow for some more variance.
    public RoomData startingRoom;
    public RoomData itemRoom;
    public RoomData bossRoom;

    [Header("Normal Rooms")]
    public RoomPool normalRoomPool;


    public OfficeMap GenerateMap()
    {
        OfficeMap map = new OfficeMap(startingRoom);

        Vector2Int center = new Vector2Int(5, 5);

        // Note: (5, 5) is the center point
        map.PlaceRoom(startingRoom, center);

        Vector2Int[] cardinalDirections = new Vector2Int[]{
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        List<Vector2Int> placeable = new List<Vector2Int>();
        foreach (Vector2Int dir in cardinalDirections)
        {
            placeable.Add(center + dir);
        }

        int iterations = 0;
        for (int i = 0; i < numRooms - 1; ++i)
        {
            ++iterations;
            if (iterations > 1000)
            {
                Debug.LogWarning("Too many map generation iterations. Stopping now!");
            }
            if (placeable.Count == 0)
            {
                Debug.LogWarning("No more placeable tiles! Ending map generation!");
                break;
            }

            // Select random position and room to generate
            int randomPosIndex = Random.Range(0, placeable.Count);
            RoomData randomRoom = normalRoomPool.GetItem();
            Vector2Int pos = placeable[randomPosIndex];

            // Add the room to the map (or try to at least)
            // Avoid clumping
            int numNearbyRooms = 0;
            foreach (Vector2Int dir in cardinalDirections)
            {
                Vector2Int newPos = pos + dir;
                if (map.HasRoom(newPos))
                {
                    ++numNearbyRooms;
                }
            }

            if (numNearbyRooms <= 1)
            {
                map.PlaceRoom(randomRoom, pos);

                // Add new placeable positions in open slots
                foreach (Vector2Int dir in cardinalDirections)
                {
                    Vector2Int newPos = pos + dir;
                    if (!map.HasRoom(newPos) && map.InBounds(newPos))
                    {
                        numNearbyRooms = 0;
                        foreach (Vector2Int dir2 in cardinalDirections)
                        {
                            if (map.HasRoom(dir2))
                            {
                                ++numNearbyRooms;
                            }
                        }
                        if (numNearbyRooms <= 1)
                        {
                            placeable.Add(newPos);
                        }
                    }
                }
            }
            else
            {
                --i;
            }

            // Remove position from the placeable list
            placeable.RemoveAt(randomPosIndex);
        }

        Debug.Log($"Map Generation completed. {iterations} iterations needed.");

        return map;
    }
}