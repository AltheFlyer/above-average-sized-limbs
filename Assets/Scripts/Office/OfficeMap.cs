
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// 
/// Note: This intentionally does not inherit from MonoBehaviour of ScriptableObject.
/// </summary>
public class OfficeMap
{

    public Room[][] rooms;

    public OfficeMap(RoomData startingRoom)
    {
        // Definitely not stealing from Isaac
        rooms = new Room[11][];
        for (int i = 0; i < 11; ++i)
        {
            rooms[i] = new Room[11];
        }
    }

    /// <summary>
    /// Returns a bool indicating success or failure of placement.
    /// </summary>
    public bool PlaceRoom(RoomData room, Vector2Int pos)
    {
        if (rooms[pos.y][pos.x] != null)
        {
            return false;
        }

        rooms[pos.y][pos.x] = new Room(room);

        return true;
    }

    /// <summary>
    /// Forcefuly sets the room at pos, overwriting any previous one.
    /// (Be careful with this one!)
    /// </summary>
    public void ForcePlaceRoom(RoomData room, Vector2Int pos)
    {
        rooms[pos.y][pos.x] = new Room(room);
    }

    public bool HasRoom(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < 11 && pos.y >= 0 && pos.y < 11)
        {
            return rooms[pos.y][pos.x] != null;
        }
        return false;
    }

    public bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 11 && pos.y >= 0 && pos.y < 11;
    }

    public Room GetRoom(Vector2Int pos)
    {
        return rooms[pos.y][pos.x];
    }
}

public class Room
{
    // Reference to the room data that defines how the 
    // room should be generated
    RoomData roomBase;

    // GameObject reference to the room within the 
    // actual game scene
    GameObject roomObject;


    public bool isVisited;

    public Room(RoomData roomBase)
    {
        this.roomBase = roomBase;
        isVisited = false;
    }

    public GameObject GetRoomPrefab()
    {
        return roomBase.roomPrefab;
    }

    public void SetLoadedRoom(GameObject roomObject)
    {
        this.roomObject = roomObject;
    }

    public void Activate()
    {
        roomObject.SetActive(true);
        Debug.Log(roomObject.GetComponent<RoomController>());
        roomObject.GetComponent<RoomController>()?.LoadRoom();
    }

    public void Deactivate()
    {
        roomObject.GetComponent<RoomController>()?.UnloadRoom();
        roomObject.SetActive(false);
    }
}
