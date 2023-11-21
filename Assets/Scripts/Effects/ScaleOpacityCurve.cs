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

    public bool destroyOnOver = true;

    Vector3 originalScale;
    float originalOpacity;

    SpriteRenderer sr;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalOpacity = sr.color.a;
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float _timer = timer / time;
        _timer = Mathf.Clamp01(_timer);

        UpdateScale(_timer);
        UpdateOpacity(_timer);

        if (_timer >= 1 && destroyOnOver) Destroy(gameObject);
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
