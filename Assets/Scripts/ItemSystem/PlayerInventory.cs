using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton containing information about the player's current items.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    // Item pickup event
    public ItemPickupEvent pickupEvent;

    // Use to display list of item names and sprites
    public List<ItemData> items = new List<ItemData>();

    // Used internally to have items receive events
    private List<BaseItem> itemListeners = new List<BaseItem>();

    private Dictionary<ItemData, BaseItem> itemToInGameListener = new Dictionary<ItemData, BaseItem>();

    // Singleton stuff
    private static PlayerInventory _instance;
    public static PlayerInventory instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
    }

    public void AddItem(ItemData item, PlayerManager player)
    {
        if (itemToInGameListener.ContainsKey(item))
        {
            itemToInGameListener[item].Stack();
            pickupEvent.Raise(item);
            return;
        }

        items.Add(item);

        BaseItem obj = Instantiate(item.item, transform);
        itemToInGameListener[item] = obj;

        itemListeners.Add(obj);

        obj.OnPickUp(player);

        pickupEvent.Raise(item);
    }

    public void PreAttack(AttackData data)
    {
        foreach (BaseItem item in itemListeners)
        {
            item.PreAttack(data);
        }
    }

    public void OnHit(HitData data)
    {
        foreach (BaseItem item in itemListeners)
        {
            item.OnHit(data);
        }
    }
}