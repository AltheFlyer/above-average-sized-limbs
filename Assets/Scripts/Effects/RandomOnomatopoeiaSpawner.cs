using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomOnomatopoeiaSpawner : MonoBehaviour
{
    [Header("Random Area")]
    public Vector2 size = Vector2.one;

    [Header("Onomatopoeia")]
    public OnomatopoeiaSet onomatopoeias;
    public GameObject basePrefab;

    public void Activate()
    {
        GameObject i = Instantiate(basePrefab, transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2)), Quaternion.identity);
        i.GetComponent<SpriteRenderer>().sprite = onomatopoeias.GetRandomSprite();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
