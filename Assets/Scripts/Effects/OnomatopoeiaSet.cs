using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnomatopoeiaSet", menuName = "Effects/OnomatopoeiaSet", order = 1)]
public class OnomatopoeiaSet : ScriptableObject
{
    public Sprite[] sprites;

    public Sprite GetSprite(int index)
    {
        return sprites[index];
    }

    public Sprite GetRandomSprite()
    {
        return sprites[Random.Range(0, sprites.Length)];
    }
}
