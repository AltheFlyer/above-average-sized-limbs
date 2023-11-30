using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffectSpawner : MonoBehaviour
{
    public static GameObject parent;

    public bool randomRotation;
    public Sprite[] spriteRandomSet;
    public Color color = Color.white;

    public void Activate()
    {
        GameObject i = new GameObject("Effect");
        i.transform.position = transform.position;

        // check if game object named "[SpriteEffectSpawner]" exists for parenting
        if (parent == null)
            parent = new GameObject("[SpriteEffectSpawner]");

        i.transform.parent = parent.transform;

        // add sprite renderer
        SpriteRenderer sr = i.AddComponent<SpriteRenderer>();
        sr.sprite = spriteRandomSet[Random.Range(0, spriteRandomSet.Length)];
        sr.color = color;
        sr.sortingLayerName = "Background";
        sr.sortingOrder = 10;

        // add transform info
        i.transform.position = transform.position;
        if (randomRotation) i.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
}
