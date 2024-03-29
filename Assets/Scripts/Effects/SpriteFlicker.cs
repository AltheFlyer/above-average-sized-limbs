using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlicker : MonoBehaviour
{
    public Material flickerMaterial;
    Material originalMaterial;

    public float flickerTime = 0.2f;

    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        originalMaterial = spriteRenderer.material;
    }

    public void Flicker()
    {
        StartCoroutine(FlickerIE());
    }

    IEnumerator FlickerIE()
    {
        spriteRenderer.material = flickerMaterial;

        yield return new WaitForSeconds(flickerTime);

        spriteRenderer.material = originalMaterial;
    }

    public void FlickerForDuration(float time)
    {
        StartCoroutine(FlickerForDurationIE(time));
    }

    IEnumerator FlickerForDurationIE(float time)
    {
        bool prevFlicker = false;
        while (time > 0)
        {
            if (prevFlicker)
            {
                spriteRenderer.material = originalMaterial;
                prevFlicker = !prevFlicker;
            }
            else
            {
                spriteRenderer.material = flickerMaterial;
                prevFlicker = !prevFlicker;
            }
            yield return new WaitForSeconds(flickerTime);
            time -= flickerTime;
        }
        spriteRenderer.material = originalMaterial;
    }

    ///<summary>When value in inspector changes, try to set sprite renderer if null</summary>
    void OnValidate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
