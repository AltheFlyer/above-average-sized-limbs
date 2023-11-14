using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackController : MonoBehaviour
{
    public LineCaster lineCaster;
    public LineAttackCaster lineAttackCaster;
    public LineRenderer lineRenderer;

    [Header("Material Visibility Graph")]
    public AnimationCurve aimVisGraph;
    public float aimVisDuration;
    public AnimationCurve fireVisGraph;
    public float fireVisDuration;
    public AnimationCurve stopVisGraph;
    public float stopVisDuration;

    [Header("End Point Particles")]
    public Transform endPoint;
    public ParticleSystem[] particles;

    void Start()
    {
        // disable everything
        lineRenderer.enabled = false;
        lineCaster.enabled = false;
        lineAttackCaster.Disable();
    }

    void Update()
    {
        // update end point
        endPoint.position = lineCaster.hitPos;
    }

    void PlayParticles()
    {
        foreach (var p in particles)
        {
            p.Play();
        }
    }

    void StopParticles()
    {
        foreach (var p in particles)
        {
            p.Stop();
        }
    }

    [ContextMenu("Aim")]
    public void Aim()
    {
        StopAllCoroutines();
        StartCoroutine(AimIE());
    }

    IEnumerator AimIE()
    {
        lineRenderer.enabled = true;
        lineCaster.enabled = true;

        StartCoroutine(VisGraphIE(aimVisGraph, aimVisDuration));

        yield return null;
    }

    [ContextMenu("Fire")]
    public void Fire()
    {
        StopAllCoroutines();
        StartCoroutine(FireIE());
    }

    IEnumerator FireIE()
    {
        lineRenderer.enabled = true;
        lineCaster.enabled = true;

        StartCoroutine(VisGraphIE(fireVisGraph, fireVisDuration));

        yield return new WaitForSeconds(0.1f);

        PlayParticles();

        yield return new WaitForSeconds(0.5f);

        lineAttackCaster.Enable();

        yield return null;
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(StopIE());
    }

    IEnumerator StopIE()
    {
        lineAttackCaster.Disable();

        lineCaster.enabled = true;

        StartCoroutine(VisGraphIE(stopVisGraph, stopVisDuration));
        yield return new WaitForSeconds(stopVisDuration);

        StopParticles();

        lineRenderer.enabled = false;
        lineCaster.enabled = false;

        yield return null;
    }

    IEnumerator VisGraphIE(AnimationCurve graph, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            lineCaster.shaderVisibility = graph.Evaluate(t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
