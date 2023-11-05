using System.Collections.Generic;
using UnityEngine;

/// Component that manages entities and things in a given room.
/// The controller manages the logic for room entry/exit events.
public class RoomController : MonoBehaviour
{

    private Room room;

    private List<GameObject> enemySpawners;

    public void Init(Room room)
    {
        this.room = room;
    }

    public void LoadRoom()
    {
        if (!room.isVisited)
        {
            room.isVisited = true;
            FirstEnterRecursive(gameObject);
        }
    }

    public void UnloadRoom()
    {

    }

    private void FirstEnterRecursive(GameObject root)
    {
        RoomActivatable activationComponent = root.GetComponent<RoomActivatable>();
        if (activationComponent)
        {
            activationComponent.OnRoomFirstEnter();
        }
        foreach (Transform t in root.transform)
        {
            FirstEnterRecursive(t.gameObject);
        }
    }
}