using System.Collections.Generic;
using UnityEngine;

/// Component that manages entities and things in a given room.
/// The controller manages the logic for room entry/exit events.
public class RoomController : MonoBehaviour
{
    public bool isBossRoom;
    public GameObject ladder;
    private Room room;
    Animator roomAnimator;

    private List<GameObject> enemySpawners = new List<GameObject>();

    public void Start()
    {
        roomAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        bool checkDoorLock = room != null ? room.isVisited && !room.isRoomCleared : false;
        if (checkDoorLock)
            UnlockDoorsWhenRoomCleared();
    }

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
            UnlockDoorsWhenRoomCleared();
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
            Debug.Log($"Found activation component {activationComponent}");
            activationComponent.OnRoomFirstEnter();
            enemySpawners.Add(root);
        }
        foreach (Transform t in root.transform)
        {
            FirstEnterRecursive(t.gameObject);
        }
    }

    private void UnlockDoorsWhenRoomCleared()
    {
        bool roomCleared = true;
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            if (!enemySpawners[i].GetComponent<RoomActivatable>().IsRoomCleared())
            {
                roomCleared = false;
                break;
            }
        }
        if (roomCleared)
        {
            OfficeExplorationController.instance.SetAllDoorLocks(false);
            if (room != null)
                room.isRoomCleared = true;
            // Spawn ladder to go to next level
            if (isBossRoom && ladder != null)
            {
                ladder.SetActive(true);
                roomAnimator.SetTrigger("roomCleared");
            }
        }
    }

}