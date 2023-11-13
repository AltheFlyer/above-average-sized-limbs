using UnityEngine;
using UnityEngine.Events;

// Doors transport the player to different rooms upon entry
public class Door : MonoBehaviour
{
    // The position of the room to send the player to
    public Vector2Int destination;
    public Vector2Int direction;

    public UnityEvent<Vector2Int, Vector2Int> onEnterCallback;


    // Internal state
    private bool isOpen; // isOpen means door is openable and not just a wall (can be locked tho)

    // Internal components
    public SpriteRenderer spriteRenderer;

    public void Start()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("You didn't attach a sprite to the door!");
        }
    }

    public void SetOpenState(bool open)
    {
        isOpen = open;
        if (open)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Better mechanism to check for player collision
        if (other.GetComponent<PlayerManager>() == null)
        {
            return;
        }
        if (!isOpen)
        {
            return;
        }

        onEnterCallback.Invoke(destination, direction);
    }

    public void SetDoorLock(bool lockState)
    {
        // Spot if this door has 1 child (it should) - locking doors
        if (transform.childCount > 0)
        {
            Transform doorCollider = transform.GetChild(0);
            doorCollider.gameObject.SetActive(lockState || !isOpen);
        }
        else
        {
            Debug.Log("No child found under Door (this shouldn't happen!).");
        }
    }
}