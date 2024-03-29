using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that generates a map on scene start.
/// </summary>
public class OfficeExplorationController : MonoBehaviour
{

    private static OfficeExplorationController _instance;
    public static OfficeExplorationController instance
    {
        get
        {
            return _instance;
        }
    }

    public OfficeGenerator generator;

    [SerializeField]
    private OfficeMap officeMap;

    [HideInInspector] public Room currentRoom;

    public Door[] doors;

    public UnityEvent<Vector2Int, Vector2Int> onRoomChange;

    // Parent of the camera gameobject
    // Used to avoid conflicts with screenshake
    public GameObject cameraParent;

    // Reference to wall colliders
    // We telport these to the room we're in
    public GameObject wallColliders;

    [Header("Decoration")]
    // Prefab to add to the room that's adjacent to the boss room
    public GameObject bossRoomIndicator;
    private bool hasCreatedBossIndicator;

    public bool isDebugMode = true;

    public void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        InstantiateMap();
    }

    public void InstantiateMap()
    {
        if (generator == null)
        {
            Debug.LogError("You forgot to set the OfficeExplorationController's generator!");
        }
        officeMap = generator.GenerateMap();

        // Initialize doors
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].onEnterCallback = onRoomChange;
        }

        // Monkey: for each room, instantiate it and then disable it.
        for (int y = 0; y < officeMap.rooms.Length; ++y)
        {
            for (int x = 0; x < officeMap.rooms[y].Length; ++x)
            {
                Room room = officeMap.rooms[y][x];
                if (room == null)
                {
                    continue;
                }

                GameObject loadedRoom = Instantiate(room.GetRoomPrefab());
                Debug.Log(loadedRoom);

                room.SetLoadedRoom(loadedRoom);
                room.location = new Vector2Int(x, y);
                loadedRoom.GetComponent<RoomController>().Init(room);

                // Move rooms to where they should be in game world space
                loadedRoom.transform.Translate(new Vector3(x * 13 - 5 * 13, y * 7 - 5 * 7, 0));

                // When debugging, it's easier to not disable every room so you can see how the map gen looks.
                // In actual gameplay, we probably want to hide every room but the one the player is currently in.
                if (!isDebugMode)
                {
                    loadedRoom.SetActive(false);
                }
            }
        }

        // Assume the starting room is at the center position (5, 5), and activate it.
        currentRoom = officeMap.GetRoom(new Vector2Int(5, 5));
        currentRoom.Activate();


        ResetDoors(new Vector2Int(5, 5));
    }

    // Called whenever a new room is entered from a given direction.
    // Should be invoked by the onRoomChange event, which should be triggered whenever a door is entered.
    public void ChangeRoom(Vector2Int newPos, Vector2Int direction)
    {
        // Deactivate current room
        currentRoom.Deactivate();

        // Activate new room
        currentRoom = officeMap.GetRoom(newPos);
        currentRoom.Activate();

        // Prepare doors
        ResetDoors(newPos);

        // Move everything important to the next room

        // Turns out I put the door collider in the right place, 
        // so the player doesn't need to be moved at all.

        // Move the camera
        // Ideally this should move its parent, so screen shake effects still work
        cameraParent.transform.localPosition = new Vector3(
            newPos.x * 13 - 5 * 13,
            newPos.y * 7 - 5 * 7,
            0
        );
        // Move the wall colliders
        wallColliders.transform.position = new Vector3(
            newPos.x * 13 - 5 * 13,
            newPos.y * 7 - 5 * 7,
            0
        );
        // Move the doors (lol)
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].transform.position += new Vector3(
                direction.x * 13,
                direction.y * 7,
                0
            );

            // If the door leads to the boss, spawn an indicator
            if (!hasCreatedBossIndicator && newPos + doors[i].direction == officeMap.bossRoomLocation)
            {
                hasCreatedBossIndicator = true;
                GameObject indicator = Instantiate(
                    bossRoomIndicator,
                    doors[i].transform.position - new Vector3(doors[i].direction.x, doors[i].direction.y, 0),
                    Quaternion.identity
                );
                officeMap.GetRoom(newPos).AddToRoom(indicator);
            }
        }

        // Iframes for leniency
        FindAnyObjectByType<PlayerManager>().GetComponent<Damageable>()?.GiveIframes(1f);
    }

    public void SetAllDoorLocks(bool lockState)
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].SetDoorLock(lockState);
        }
    }

    // Enables and disables doors for the current room.
    // Doors are only enabled if there's a room in that direction.
    private void ResetDoors(Vector2Int newPos)
    {
        for (int i = 0; i < doors.Length; ++i)
        {
            if (officeMap.HasRoom(newPos + doors[i].direction))
            {
                doors[i].destination = newPos + doors[i].direction;
                doors[i].SetOpenable(true);
            }
            else
            {
                doors[i].SetOpenable(false);
            }
        }
        SetAllDoorLocks(!currentRoom.isRoomCleared); // set all door lock state upon entrance
    }

    public bool isCurrentlyBossRoom()
    {
        return currentRoom.location == officeMap.bossRoomLocation;
    }
}
