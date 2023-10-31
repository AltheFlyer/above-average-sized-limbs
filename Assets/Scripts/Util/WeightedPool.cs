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

    // Only used to cache the sum of weights
    private int _weightSum = 0;

    public T GetItem()
    {
        float rand = UnityEngine.Random.Range(0, GetWeightSum());

        for (int i = 0; i < items.Length; ++i)
        {
            rand -= items[i].weight;
            if (rand < 0)
            {
                return items[i].item;
            }
        }

        // Should be unreachable
        throw new InvalidOperationException("Random pool generation failed");
    }

    /// <summary>
    /// Returns the sum of weights for all items in the pool. 
    /// Computes the sum if it hasn't been computed already.
    /// </summary>
    private int GetWeightSum()
    {
        if (_weightSum == 0)
        {
            foreach (WeightedPoolEntry<T> entry in items)
            {
                _weightSum += entry.weight;
            }
        }
        return _weightSum;
    }
}

[Serializable]
public class WeightedPoolEntry<T>
{
    public T item;
    public int weight;
}