using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrenade : MonoBehaviour
{
    public GameObject marker;
    public float markerTime = 1f;
    public GameObject impact;
    public Collider2D impactCollider;
    public float impactTime = 0.2f;
    public Shooter smallHandShooter;
    public float smallHandSpawnerTime = 0.5f;

    void Start()
    {
        StartCoroutine(MainIE());
    }

    IEnumerator MainIE()
    {
        marker.SetActive(true);

        yield return new WaitForSeconds(markerTime);

        impact.SetActive(true);
        if (impactCollider != null) impactCollider.enabled = true;

        yield return new WaitForSeconds(impactTime);

        smallHandShooter.Fire();
        impactCollider.enabled = false;

        yield return new WaitForSeconds(smallHandSpawnerTime);

        Destroy(gameObject);
    }
}
