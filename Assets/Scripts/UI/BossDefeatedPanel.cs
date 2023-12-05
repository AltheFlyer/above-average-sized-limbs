using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDefeatedPanel : MonoBehaviour
{
    public Image main;
    public Image[] subs;

    [Space]
    public float fadeTime = 1f;
    public AnimationCurve mainFadeCurve;
    public AnimationCurve subFadeCurve;

    void Start()
    {
        main.enabled = false;
        foreach (var sub in subs)
            sub.enabled = false;
    }

    public void CheckBossHP(BossHPUIData data)
    {
        if (data.hpRatio == 0)
        {
            Activate();
        }
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateIE());
    }

    IEnumerator ActivateIE()
    {
        // enable all
        main.enabled = true;
        foreach (var sub in subs)
            sub.enabled = true;

        // set timer
        float timer = 0;

        // evaluate curves
        while (timer < fadeTime)
        {
            float ratio = timer / fadeTime;

            float a = mainFadeCurve.Evaluate(ratio);
            main.color = new Color(main.color.r, main.color.g, main.color.b, a);
            a = subFadeCurve.Evaluate(ratio);
            foreach (var sub in subs)
            {
                sub.color = new Color(sub.color.r, sub.color.g, sub.color.b, a);
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // disable all
        main.enabled = false;
        foreach (var sub in subs)
            sub.enabled = false;
    }
}
