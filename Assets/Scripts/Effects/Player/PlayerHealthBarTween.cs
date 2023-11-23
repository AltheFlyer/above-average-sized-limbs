using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarTween : MonoBehaviour
{
    private Vector3 originalScale;
    public float tweenTime = 0.2f;
    public float tweenScale = 1.2f;

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
    }
}
