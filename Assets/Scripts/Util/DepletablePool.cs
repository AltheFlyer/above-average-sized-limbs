using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic base class for random pools where items are not replaced when drawn. 
/// You MUST call RefreshPool before using it (to avoid ScriptableObject permanence from kicking in)
/// </summary>
public class DepletablePool<T> : ScriptableObject, IRandomPool<T>
{
    [SerializeField]
    private WeightedPoolEntry<T>[] initialItems;

    [SerializeField]
    private T defaultItem;

    // Actual item pools used for draws - should eventually run out
    private List<WeightedPoolEntry<T>> currentItems;

    public void RefreshPool()
    {
        currentItems.Clear();

        currentItems.AddRange(initialItems);
    }

    public T GetItem()
    {
        if (GetWeightSum() == 0)
        {
            return defaultItem;
        }

        int rand = UnityEngine.Random.Range(0, GetWeightSum());

        int currSum = 0;
        for (int i = 0; i < currentItems.Count; ++i)
        {
            currSum += currentItems[i].weight;
            if (rand < currSum)
            {
                T res = currentItems[i].item;
                currentItems.RemoveAt(i);
                return res;
            }
        }

        // Should be unreachable
        Debug.LogWarning($"Could not generate item with random value {rand}. Pool sum {GetWeightSum()}");
        throw new InvalidOperationException("Random pool generation failed");
    }

    /// <summary>
    /// Returns the sum of weights for all items in the pool. 
    /// Computes the sum if it hasn't been computed already.
    /// </summary>
    private int GetWeightSum()
    {
        int weightSum = 0;
        foreach (WeightedPoolEntry<T> entry in currentItems)
        {
            weightSum += entry.weight;
        }
        return weightSum;
    }
}

[Serializable]
public class DepletedPoolEntry<T>
{
    public T item;
    public int weight;
}