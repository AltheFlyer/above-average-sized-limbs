using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modified OfficeGenerator that accounts for rooms that cannot have all four doors.
/// </summary>
[CreateAssetMenu(menuName = "Office/Office Generator MK2")]
public class OfficeGeneratorMK2 : OfficeGenerator
{
    // Generates an OfficeMap object, containing a grid of rooms
    public override OfficeMap GenerateMap()
    {
        // Basically an 11 by 11 grid of room info
        OfficeMap map = new OfficeMap(startingRoom);

        // Hardcoded constant value: position (5, 5) is the center of an 11 by 11 grid
        Vector2Int center = new Vector2Int(5, 5);

        // The starting room is put at the center of the grid
        map.PlaceRoom(startingRoom, center);

        // List to store positions that rooms can be placed in.
        // Rooms will only be placed next to other preexisting rooms.
        List<Vector2Int> placeable = new List<Vector2Int>();
        // Add the positions adjacent to the center (starting) room to the placeable list
        foreach (Vector2Int dir in startingRoom.doorLocations)
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

            // Check if the new room can actually have a door connecting to the room beside it.
            // This assumes we always place new rooms beside an existing one.
            bool canPlaceFlag = true;
            foreach (Vector2Int dir in cardinalDirections)
            {
                if (map.HasRoom(dir + pos))
                {
                    if (!randomRoom.doorLocations.Contains(dir))
                    {
                        canPlaceFlag = false;
                    }
                }
            }

            // Avoid clumping; don't place the room if it would be beside 
            // two or more other rooms.
            if (canPlaceFlag && NumNeighbours(map, pos) <= 1)
            {
                // If room clumping isn't a problem, add the room to the map
                map.PlaceRoom(randomRoom, pos);

                // Add new placeable positions, only if they satisfy some conditions:
                // - The position is in the 11 by 11 grid
                // - The position is not already occupied by a room
                // - The position is not adjacent to two or more rooms
                // Consequence: We will never place a new room so that it is 
                // beside two or more preexisting rooms, but we may allow more than two 
                // new rooms to be placed beside a preexisting one.
                foreach (Vector2Int dir in randomRoom.doorLocations)
                {
                    Vector2Int newPos = pos + dir;
                    if (!map.HasRoom(newPos) && map.InBounds(newPos))
                    {
                        if (NumNeighbours(map, newPos) <= 1)
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

        PlaceSpecialRooms(map);

        Debug.Log($"Map Generation completed. {iterations} iterations needed.");

        return map;
    }

    /// <summary>
    /// Replaces the furthest dead-end room from the center with the boss room.
    /// Replaces the second-furthest dead-end room from the center with the item room.
    /// A dead-end room is a room with only one neighbour.
    /// </summary>
    public override void PlaceSpecialRooms(OfficeMap map)
    {
        if (bossRoom.doorLocations.Count < 4 || itemRoom.doorLocations.Count < 4)
        {
            Debug.LogWarning("WARNING! Map Generator currently requires the item and boss room to support doors in any direction!" +
                "(Yell at Allen to fix this is you want this changed).");
        }
        base.PlaceSpecialRooms(map);
    }
}