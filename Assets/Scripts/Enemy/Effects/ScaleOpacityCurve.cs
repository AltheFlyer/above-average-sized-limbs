using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScaleOpacityCurve : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    public AnimationCurve opacityCurve;
    public float time = 1;
    float timer = 0;

    Vector3 originalScale;
    float originalOpacity;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalOpacity = sr.color.a;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float _timer = timer / time;

        UpdateScale(_timer);
        UpdateOpacity(_timer);

        if (_timer >= 1) Destroy(gameObject);
    }

    void UpdateScale(float normalizedTime)
    {
        float scale = scaleCurve.Evaluate(normalizedTime);

        transform.localScale = originalScale * scale;
    }

    void UpdateOpacity(float normalizedTime)
    {
        float opacity = opacityCurve.Evaluate(normalizedTime);

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, opacity * originalOpacity);
    }
}
