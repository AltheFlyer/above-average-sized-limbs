using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject containing information to generate a office map.
/// (Basically, the map of rooms the player will have to go through).
/// The player begins in a starting room, and other rooms are generated based 
/// on the provided normalRoomPool. 
/// A single item and boss room are also placed within the map.
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


    protected Vector2Int[] cardinalDirections = new Vector2Int[]{
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left,
        };

    // Generates an OfficeMap object, containing a grid of rooms
    public virtual OfficeMap GenerateMap()
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

            // Avoid clumping; don't place the room if it would be beside 
            // two or more other rooms.
            if (NumNeighbours(map, pos) <= 1)
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
                foreach (Vector2Int dir in cardinalDirections)
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
    public virtual void PlaceSpecialRooms(OfficeMap map)
    {
        int maxDist = 0;
        int secondMaxDist = 0;
        Vector2Int secondFurthestDeadEnd = new Vector2Int(5, 5);
        Vector2Int furthestDeadEnd = new Vector2Int(5, 5);
        for (int y = 0; y < 11; ++y)
        {
            for (int x = 0; x < 11; ++x)
            {
                int dist = Mathf.Abs(y - 5) + Mathf.Abs(x - 5);
                if (map.rooms[y][x] != null &&
                    NumNeighbours(map, new Vector2Int(x, y)) == 1)
                {
                    if (dist > maxDist)
                    {
                        secondMaxDist = maxDist;
                        maxDist = dist;
                        secondFurthestDeadEnd = furthestDeadEnd;
                        furthestDeadEnd = new Vector2Int(x, y);
                    }
                    else if (dist > secondMaxDist)
                    {
                        secondMaxDist = dist;
                        secondFurthestDeadEnd = new Vector2Int(x, y);
                    }
                }
            }
        }

        // If we couldn't place the item or boss rooms, uh... try to spawn a new room just for it
        // I have no formal proof, but I'm pretty sure that a given map with more than one room must have 
        // at least two dead ends, since rooms are never placed in a way that decreases the number of them.
        // Since we only place a room if it would only be adjacent to one preexisting room, 
        // we never allow a new room to "convert" two dead-end rooms into non-dead end rooms.
        // Any time we remove a dead-end room, it's to place a new room, which must have exactly one neighbour, 
        // meaning the total number of dead ends is still unchanged.
        // However, in the degenerate case, it's possible we created a line ending at the spawn room, 
        // which we really don't want to replace. Should this be the case, we can simply 
        // add a room beside the spawn room to solve the problem.
        // But just in case I screwed up, uhhh throw an exception because something really bad happened.
        if (furthestDeadEnd.x == 5 && furthestDeadEnd.y == 5)
        {
            if (!TryPlaceRoomBeside(map, new Vector2Int(5, 5), bossRoom))
            {
                throw new System.Exception("Couldn't place boss room! Panicking!");
            }
        }
        else
        {
            map.ForcePlaceRoom(bossRoom, furthestDeadEnd);
        }
        if (secondFurthestDeadEnd.x == 5 && secondFurthestDeadEnd.y == 5)
        {
            if (!TryPlaceRoomBeside(map, new Vector2Int(5, 5), itemRoom))
            {
                throw new System.Exception("Couldn't place item room! Panicking!");
            }
        }
        else
        {
            map.ForcePlaceRoom(itemRoom, secondFurthestDeadEnd);
        }
    }

    /// <summary>
    /// Returns the number of neighbouring rooms to the room at position (x, y)
    /// </summary>
    protected virtual int NumNeighbours(OfficeMap map, Vector2Int pos)
    {
        int neighbours = 0;
        foreach (Vector2Int dir in cardinalDirections)
        {
            Vector2Int newPos = pos + dir;
            if (map.HasRoom(newPos))
            {
                ++neighbours;
            }
        }
        return neighbours;
    }

    /// <summary>
    /// Tries to place a specified room beside a specific position in the map.
    /// Assuming the provided pos contains a room already, this will only try to place rooms 
    /// if the room at pos is the only neighbour.
    /// Returns a boolean indicating if the attempt was successful or not.
    /// </summary>
    protected virtual bool TryPlaceRoomBeside(OfficeMap map, Vector2Int pos, RoomData roomToPlace)
    {
        foreach (Vector2Int dir in cardinalDirections)
        {
            Vector2Int newPos = pos + dir;
            if (!map.HasRoom(newPos) && NumNeighbours(map, newPos) == 1)
            {
                map.PlaceRoom(roomToPlace, newPos);
                return true;
            }
        }
        return false;
    }
}