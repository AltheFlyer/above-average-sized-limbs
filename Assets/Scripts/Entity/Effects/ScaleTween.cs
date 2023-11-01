using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    private Vector3 originalScale;
    public float tweenTime = 0.2f;
    public float tweenScale = 1.2f;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Tween()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = originalScale;

        LeanTween.scale(gameObject, originalScale * tweenScale, tweenTime)
            .setEasePunch();
    }
}
