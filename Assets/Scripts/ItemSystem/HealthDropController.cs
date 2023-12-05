using UnityEngine;

public class HealthDropController : MonoBehaviour
{
    // The chance an enemy drops something, if its drop rate isn't defined
    public float dropRate = 0.1f;

    public ItemPool itemPool;

    public ItemPickup pickupPrefab;

    // Use in event listener
    public void SpawnItem(EnemyDeathData data)
    {
        if (Random.Range(0.0f, 1.0f) > dropRate)
        {
            return;
        }

        ItemData itemToSpawn = itemPool.GetItem();

        ItemPickup newPickup = Instantiate(pickupPrefab, data.deathLocation, Quaternion.identity);
        newPickup.item = itemToSpawn;
    }
}