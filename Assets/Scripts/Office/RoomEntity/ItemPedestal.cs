using UnityEngine;

/// <summary>
/// Indicates where a random item can generate in a room. 
/// Generates the item when the room is loaded, in accordance to the item pool.
/// </summary>
public class ItemPedestal : RoomActivatable
{
    public ItemPool itemPool;

    public ItemPickup pickupPrefab;

    public override void OnRoomFirstEnter()
    {
        ItemData itemToSpawn = itemPool.GetItem();

        ItemPickup newPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity, transform);
        newPickup.item = itemToSpawn;
    }

    public override bool IsRoomCleared()
    {
        return true;
    }
}