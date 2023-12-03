using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableFadeObject : MonoBehaviour
{
    public float time = 3f;

    void Start()
    {
        StartCoroutine(FadeIE());
    }

    IEnumerator FadeIE()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
