using UnityEngine;

public class RandomDropController : MonoBehaviour
{
    // The chance an enemy drops something, if its drop rate isn't defined
    public float defaultDropRate = 0.05f;

    // multiplier applied to an enemy's drop rate
    // Use values < 1 to drop fewer items than usual, >1 for more
    public float dropRateMultiplier = 1.0f;

    public ItemPool itemPool;

    public ItemPickup pickupPrefab;

    // Use in event listener
    public void SpawnItem(EnemyDeathData data)
    {
        float chance = defaultDropRate;
        if (data.enemy.GetComponent<DropRate>() != null)
        {
            chance = data.enemy.GetComponent<DropRate>().itemDropRate;
        }
        chance *= dropRateMultiplier;

        Debug.Log($"Drop chance: {chance}");

        if (Random.Range(0.0f, 1.0f) > chance)
        {
            return;
        }

        ItemData itemToSpawn = itemPool.GetItem();

        ItemPickup newPickup = Instantiate(pickupPrefab, data.deathLocation, Quaternion.identity);
        newPickup.item = itemToSpawn;
    }
}