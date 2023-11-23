using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAttack : MonoBehaviour
{
    public GameObject marker;
    public float markerTime = 1f;
    public GameObject impact;
    public Collider2D impactCollider;
    public float impactTime = 2.3f;

    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        StartCoroutine(MainIE());
    }

    IEnumerator MainIE()
    {
        marker.SetActive(true);

        yield return new WaitForSeconds(markerTime);

        impact.SetActive(true);
        impactCollider.enabled = true;
        SFXManager.TryPlaySFX("bomb1", gameObject);

        yield return new WaitForSeconds(impactTime);

        impactCollider.enabled = false;

        Destroy(gameObject);
    }
}
