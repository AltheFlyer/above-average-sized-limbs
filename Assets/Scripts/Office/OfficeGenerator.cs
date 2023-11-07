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

    // Currently unused
    // Intention: at some point, we'll use these to 
    // generate the specified item and boss rooms
    public RoomData itemRoom;
    public RoomData bossRoom;

    [Header("Normal Rooms")]
    public RoomPool normalRoomPool;


    // Generates an OfficeMap object, containing a grid of rooms
    public OfficeMap GenerateMap()
    {
        // Basically an 11 by 11 grid of room info
        OfficeMap map = new OfficeMap(startingRoom);

        // Hardcoded constant value: position (5, 5) is the center of an 11 by 11 grid
        Vector2Int center = new Vector2Int(5, 5);

        // The starting room is put at the center of the grid
        map.PlaceRoom(startingRoom, center);

        // Also hardcoded constant, just vectors for each cardinal direction
        Vector2Int[] cardinalDirections = new Vector2Int[]{
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

        // List to store positions that rooms can be placed in.
        // Rooms will only be placed next to other preexisting rooms.
        List<Vector2Int> placeable = new List<Vector2Int>();
        // Add the positions adjacent to the center (starting) room to the placeable list
        foreach (Vector2Int dir in cardinalDirections)
        {
            placeable.Add(center + dir);
        }

        // Loop to (attempt to) generate rooms for the floor.
        // In case our luck is so bad that we keep trying to place invalid rooms, 
        // we escape the loop if we've iterated thru the loop too many times.
        int iterations = 0;
        for (int i = 0; i < numRooms - 1; ++i)
        {
            ++iterations;
            // Escape if we did too many loops without spawning all the rooms
            if (iterations > 1000)
            {
                Debug.LogWarning("Too many map generation iterations. Stopping now!");
            }
            // Escape the loop if there are no valid spots to place rooms in
            if (placeable.Count == 0)
            {
                Debug.LogWarning("No more placeable tiles! Ending map generation!");
                break;
            }

            // Select random position and room to generate at that position
            int randomPosIndex = Random.Range(0, placeable.Count);
            RoomData randomRoom = normalRoomPool.GetItem();
            Vector2Int pos = placeable[randomPosIndex];

            // Add the room to the map (or try to at least)

            // Avoid clumping - count the number of rooms adjacent to our 
            // placement loction.
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
                // If room clumping isn't a problem, add the room to the map
                map.PlaceRoom(randomRoom, pos);

                // Add new placeable positions, only if they satisfy some conditions:
                // - The position is in the 11 by 11 grid
                // - The position is not already occupied by a room
                // - The position is not adjacent to two or more rooms
                // Consequence: We will never place a new room so that it is 
                // beside two or more preexisting rooms, but we may allow more than two rooms 
                // to be placed beside a preexisting one.
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
                // Jank, modify the iterator variable since we didn't actually place a room
                --i;
            }

            // Remove position from the placeable list
            placeable.RemoveAt(randomPosIndex);
        }

        Debug.Log($"Map Generation completed. {iterations} iterations needed.");

        return map;
    }
}