using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodyScreen : MonoBehaviour
{
    public Image image;

    public float splatterInitialAlpha = 0.3f;
    public float splatterTime = 0.6f;

    void Start()
    {
        image.enabled = false;
    }

    public void Splatter()
    {
        image.enabled = false;
        StopAllCoroutines();
        StartCoroutine(SplatterIE());
    }

    IEnumerator SplatterIE()
    {
        image.enabled = true;

        float a = splatterInitialAlpha;
        float timer = splatterTime;
        float rate = splatterInitialAlpha / splatterTime;

        while (timer > 0)
        {
            a -= rate * Time.deltaTime;

            image.color = new Color(image.color.r, image.color.g, image.color.b, a);

            timer -= Time.deltaTime;
            yield return null;
        }

        image.enabled = false;
    }
}
