using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarTween : MonoBehaviour
{
    private Vector3 originalScale;
    public float tweenTime = 0.2f;
    public float tweenScale = 1.2f;
    public float flickerTime = 0.3f;
    public Material flickerMaterial;

    RectTransform rectTransform;

    public static PlayerHealthBarTween instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    [ContextMenu("Tween")]
    public void Tween()
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(rectTransform, originalScale * tweenScale, tweenTime)
            .setEasePunch();

        StartCoroutine(WhiteFlicker());
    }

    IEnumerator WhiteFlicker()
    {
        // get all child image object
        List<Image> images = new List<Image>();
        foreach (Transform c in transform)
        {
            if (c.TryGetComponent(out Image image))
            {
                images.Add(image);
            }
        }

        Material originalMaterial = images[0].material;

        // set all image to white
        foreach (var i in images)
        {
            if (i != null)
                i.material = flickerMaterial;
        }

        yield return new WaitForSeconds(flickerTime);

        // set all image material back
        foreach (var i in images)
        {
            if (i != null)
                i.material = originalMaterial;
        }
    }
}
