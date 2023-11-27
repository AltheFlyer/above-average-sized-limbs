using UnityEngine;

/// <summary>
/// Component for objects in game that give the player items 
/// when they walk over it.
/// Eventually, this should be repurposed into something that 
/// creates a Hades-style screen, but the inventory interaction 
/// should be pretty much the same.
/// </summary>
public class ItemPickup : MonoBehaviour
{

    public ItemData item;

    public SpriteRenderer itemSpriteRenderer;

    public void Start()
    {
        if (itemSpriteRenderer == null)
        {
            itemSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (item.additionalRendererOnPickup != null)
        {
            Instantiate(item.additionalRendererOnPickup, transform.position, Quaternion.identity, transform);
        }
        else
        {
            itemSpriteRenderer.sprite = item.sprite;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager player = other.GetComponent<PlayerManager>();
        // TODO: Better mechanism to check for player collision
        if (player == null)
        {
            return;
        }

        PlayerInventory.instance?.AddItem(item, player);
        Destroy(gameObject);
    }
}
