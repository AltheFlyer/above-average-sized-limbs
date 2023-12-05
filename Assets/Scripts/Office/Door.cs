using UnityEngine;
using UnityEngine.Events;

// Doors transport the player to different rooms upon entry
public class Door : MonoBehaviour
{
    // The position of the room to send the player to
    public Vector2Int destination;
    public Vector2Int direction;

    public UnityEvent<Vector2Int, Vector2Int> onEnterCallback;

    public SpriteRenderer spriteRenderer;

    public GameObject[] activeIfOpen;
    public GameObject[] activeIfClosed;


    // Internal state

    // Indicates whether or not the door canconnects to a different room
    // If `isDoorOpenable` is true, then the door can be locked or unlocked.
    // Otherwise, the door is always locked (effectively just a wall).
    private bool isDoorOpenable;

    public void Start()
    {
    }

    public void SetOpenable(bool openable)
    {
        isDoorOpenable = openable;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Disable trigger if the door is actually a wall
        if (!isDoorOpenable)
        {
            return;
        }

        // Only activate on player entry
        if (other.GetComponent<PlayerManager>() == null)
        {
            return;
        }

        onEnterCallback.Invoke(destination, direction);
    }

    public void SetDoorLock(bool lockState)
    {
        // Enable/disable children
        if (transform.childCount > 0)
        {
            foreach (GameObject go in activeIfOpen)
            {
                // Today, I will use de Morgan's Law. (Clueless)
                go.gameObject.SetActive(!lockState && isDoorOpenable);
            }
            foreach (GameObject go in activeIfClosed)
            {
                go.gameObject.SetActive(lockState || !isDoorOpenable);
            }
        }
        else
        {
            Debug.Log("No child found under Door (this shouldn't happen!).");
        }
    }
}