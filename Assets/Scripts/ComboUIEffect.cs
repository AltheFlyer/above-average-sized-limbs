using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;

public class ComboUIEffect : MonoBehaviour
{
    public IntVariable comboCount;
    public TextMeshProUGUI comboText;
    private bool isTextShaking = false;
    public float[] baseJitterParams;
    private float[] activeJitterParams;
    private Vector3 originalPosition;
    private int[] comboThreshold = new int[2];
    public VisualEffect comboVFX;

    void Start()
    {
        //currentComboText = Instantiate(comboText, this.transform.position, Quaternion.identity, transform);
        comboText.gameObject.SetActive(true);
        originalPosition = comboText.transform.localPosition;
        activeJitterParams = new float[2];
        comboThreshold[0] = 3;
        comboThreshold[1] = 6;
    }
    void Update()
    {
        ShowComboEffect();
        if (isTextShaking)
        {
            ApplyJitter(activeJitterParams[0], activeJitterParams[1]);
        }

    }


    public void ShowComboEffect()
    {
        Color comboTextColor;

        if (comboCount.Value <= comboThreshold[0])
        {
            comboTextColor = Color.white;
            isTextShaking = false;
            comboText.transform.localPosition = originalPosition;
            comboVFX.enabled = false;
        }
        else if (comboCount.Value <= comboThreshold[1] && comboCount.Value > comboThreshold[0])
        {
            comboTextColor = Color.yellow;
            isTextShaking = true;
            activeJitterParams[0] = baseJitterParams[0];
            activeJitterParams[1] = baseJitterParams[1];
            comboVFX.enabled = true;
        }
        else
        {
            comboTextColor = Color.red;
            isTextShaking = true;
            activeJitterParams[0] = baseJitterParams[0] * 5;
            activeJitterParams[1] = baseJitterParams[1] * 5;
            comboVFX.enabled = true;
        }

        comboText.color = comboTextColor;
        comboText.text = "Combo: " + comboCount.Value.ToString();
    }

    void ApplyJitter(float jitterAmount, float jitterSpeed)
    {
        float jitterX = Mathf.PerlinNoise(Time.time * jitterSpeed, 0) * 2 - 1;
        float jitterY = Mathf.PerlinNoise(0, Time.time * jitterSpeed) * 2 - 1;

        Vector3 jitterOffset = new Vector3(jitterX, jitterY, 0) * jitterAmount;
        comboText.transform.localPosition = originalPosition + jitterOffset;
    }
}
