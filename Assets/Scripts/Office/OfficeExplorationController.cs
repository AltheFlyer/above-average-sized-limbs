using UnityEngine;
using UnityEngine.Events;

public class OfficeExplorationController : MonoBehaviour
{

    public OfficeGenerator generator;

    [SerializeField]
    private OfficeMap officeMap;

    private Room currentRoom;

    public Door[] doors;

    public UnityEvent<Vector2Int, Vector2Int> onRoomChange;

    public Camera camera;

    public void Start()
    {
        officeMap = generator.GenerateMap();

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

                room.SetLoadedRoom(loadedRoom);
                loadedRoom.GetComponent<RoomController>()?.Init(room);

                loadedRoom.transform.Translate(new Vector3(x * 13 - 5 * 13, y * 7 - 5 * 7, 0));

                // loadedRoom.SetActive(false);
            }
        }

        currentRoom = officeMap.GetRoom(new Vector2Int(5, 5));

        currentRoom.Activate();

        for (int i = 0; i < doors.Length; ++i)
        {
            if (officeMap.HasRoom(new Vector2Int(5, 5) + doors[i].direction))
            {
                doors[i].gameObject.SetActive(true);
                doors[i].destination = new Vector2Int(5, 5) + doors[i].direction;
                doors[i].SetOpenState(true);
            }
            else
            {
                doors[i].SetOpenState(false);
            }
        }
    }

    public void ChangeRoom(Vector2Int newPos, Vector2Int direction)
    {
        Debug.Log($"Moving to room at {newPos}");
        // Deactivate current room
        currentRoom.Deactivate();

        // Activate new room
        currentRoom = officeMap.GetRoom(newPos);
        currentRoom.Activate();

        // Prepare doors
        for (int i = 0; i < doors.Length; ++i)
        {
            if (officeMap.HasRoom(newPos + doors[i].direction))
            {
                doors[i].destination = newPos + doors[i].direction;
                doors[i].SetOpenState(true);
            }
            else
            {
                doors[i].SetOpenState(false);
            }
        }

        // Move everything important to the next room
        // Move the player to the appropriate entrance
        GameObject.FindObjectOfType<PlayerManager>().transform.position += new Vector3(
            0,
            0,
            0
        );
        // Move the camera
        camera.transform.position = new Vector3(
            newPos.x * 13 - 5 * 13,
            newPos.y * 7 - 5 * 7,
            -10
        );
        // Move the doors (lol)
        for (int i = 0; i < doors.Length; ++i)
        {
            doors[i].transform.position += new Vector3(
                direction.x * 13,
                direction.y * 7,
                0
            );
        }
    }
}