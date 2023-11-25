using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class that initializes data that might otherwise have been modified from previous game sessions.
/// Any time you need to set a ScriptableObject's fields on the game's first run, do it here.
/// </summary>
public class SharedDataInitializer : MonoBehaviour
{

    public PlayerVariables baseStats;
    public PlayerVariables activeStats;

    public List<DepletablePool<ItemData>> itemPools;

    public void Awake()
    {
        // Initalize player stats to starting values
        baseStats.CopyInto(activeStats);

        // Reset item pools
        foreach (DepletablePool<ItemData> pool in itemPools)
        {
            pool.RefreshPool();
        }
    }
}
