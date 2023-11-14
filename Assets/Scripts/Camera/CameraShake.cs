using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShake : Singleton<CameraShake>
{
    public float shakeFrequency = 15;
    private float magnitude;
    public float magnitudeDegradation = 0.93f;

    Vector3 originalPos;

    public override void Awake()
    {
        base.Awake();
        originalPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = originalPos + GetOffset();
    }

    void FixedUpdate()
    {
        magnitude *= magnitudeDegradation;
    }

    public void Shake(float mag = 1)
    {
        magnitude = mag * 0.08f;
    }

    Vector3 GetOffset()
    {
        float _mag = magnitude * Mathf.Sin(Time.time * shakeFrequency);
        Vector2 _out = Vector2.one * _mag;
        return new Vector3(_out.x, _out.y, originalPos.z);
    }
}
