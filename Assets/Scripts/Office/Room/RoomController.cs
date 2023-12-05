using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Component that manages entities and things in a given room.
/// The controller manages the logic for room entry/exit events.
public class RoomController : MonoBehaviour
{
    public bool isBossRoom;
    public GameObject ladder;
    private Room room;
    Animator roomAnimator;

    private int enemyCount = 0;

    public void Start()
    {
        roomAnimator = GetComponent<Animator>();
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
            // Check enemy spawns on the next frame
            StartCoroutine(NextFrameDoorCheck());
        }
    }

    IEnumerator NextFrameDoorCheck()
    {
        yield return null;
        UnlockDoorsWhenRoomCleared();
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

    private void UnlockDoorsWhenRoomCleared()
    {
        if (enemyCount == 0)
        {
            SFXManager.TryPlaySFX("unlock1", gameObject);
            OfficeExplorationController.instance.SetAllDoorLocks(false);
            if (room != null)
                room.isRoomCleared = true;
            // Spawn ladder to go to next level
            if (isBossRoom && ladder != null)
            {
                ladder.SetActive(true);
                roomAnimator?.SetTrigger("roomCleared");
            }
            else if (isBossRoom && ladder == null)
            {
                // No ladder in room means final boss
                SceneManager.LoadSceneAsync("End", LoadSceneMode.Single);
            }
        }
    }

    public void OnEnemyDeath(EnemyDeathData data)
    {
        enemyCount--;
        UnlockDoorsWhenRoomCleared();
    }

    public void OnEnemySpawn(EnemySpawnData data)
    {
        enemyCount++;
    }
}
