using UnityEngine;

public class OfficeExplorationController : MonoBehaviour
{

    public OfficeGenerator generator;

    [SerializeField]
    private OfficeMap officeMap;

    private Room currentRoom;

    public void Start()
    {
        officeMap = generator.GenerateMap();

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

                loadedRoom.transform.Translate(new Vector3(x * 13 - 5 * 13, y * 7 - 5 * 7, 0));

                // loadedRoom.SetActive(false);
            }
        }

        currentRoom = officeMap.GetRoom(new Vector2Int(5, 5));

        currentRoom.Activate();
    }

    public void ChangeRoom(Vector2Int newPos, Vector2Int direction)
    {
        currentRoom.Deactivate();

        currentRoom = officeMap.GetRoom(newPos);
        currentRoom.Activate();
    }
}