using System;
using UnityEngine;

/// <summary>
/// Generic base class for random pools used in the game.
/// T is a type parameter for the object the pool should contain, 
/// and the class extends ScriptableObject to allow for creation of
/// pools in the Unity Editor.
/// </summary>
public class WeightedPool<T> : ScriptableObject, IRandomPool<T>
{
    [SerializeField]
    private WeightedPoolEntry<T>[] items;

    public T GetItem()
    {
        int rand = UnityEngine.Random.Range(0, GetWeightSum());

        int currSum = 0;
        for (int i = 0; i < items.Length; ++i)
        {
            currSum += items[i].weight;
            if (rand < currSum)
            {
                return items[i].item;
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
        foreach (WeightedPoolEntry<T> entry in items)
        {
            weightSum += entry.weight;
        }
        return weightSum;
    }
}

[Serializable]
public class WeightedPoolEntry<T>
{
    public T item;
    public int weight;
}