using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeGenerator : ScriptableObject
{

    [Header("Special, predefined rooms")]
    // For now, the special rooms are fixed for simplicity, 
    // but feel free to change the type to RoomPool(s) to allow for some more variance.
    public Room startingRoom;
    public Room itemRoom;
    public Room bossRoom;

    [Header("Normal Rooms")]
    public RoomPool normalRoomPool;
}