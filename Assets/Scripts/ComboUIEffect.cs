using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;

public class ComboUIEffect : MonoBehaviour
{
    public PlayerVariables playerVars;
    public IntVariable comboCount;
    public TextMeshProUGUI comboText;
    private int comboThreshold1;
    private int comboThreshold2;
    public float[] baseJitterParams;
    private float[] activeJitterParams;
    private Vector3 originalPosition;
    private bool isTextShaking = false;
    //private int[] comboThreshold = new int[2];
    private VisualEffect comboVFX;

    void Start()
    {
        //currentComboText = Instantiate(comboText, this.transform.position, Quaternion.identity, transform);
        comboText.gameObject.SetActive(true);
        originalPosition = comboText.transform.localPosition;
        activeJitterParams = new float[2];
        comboThreshold1 = playerVars.comboThreshold1;
        comboThreshold2 = playerVars.comboThreshold2;

        comboVFX = GameObject.Find("Combo VFX").GetComponent<VisualEffect>();
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

        if (comboCount.Value < comboThreshold1)
        {
            comboTextColor = Color.white;
            isTextShaking = false;
            comboText.transform.localPosition = originalPosition;
            comboVFX.enabled = false;
        }
        else if (comboCount.Value <= comboThreshold2 && comboCount.Value >= comboThreshold1)
        {
            comboTextColor = Color.yellow;
            isTextShaking = true;
            activeJitterParams[0] = baseJitterParams[0];
            activeJitterParams[1] = baseJitterParams[1];
            comboVFX.enabled = true;
            comboVFX.SetFloat("SpawnRate", 220f);
        }
        else
        {
            comboTextColor = Color.red;
            isTextShaking = true;
            activeJitterParams[0] = baseJitterParams[0] * 5;
            activeJitterParams[1] = baseJitterParams[1] * 5;
            comboVFX.enabled = true;
            comboVFX.SetFloat("SpawnRate", 600f);
        }

        comboText.color = comboTextColor;
        comboText.text = "Combo: " + comboCount.Value.ToString("D3");
    }

    void ApplyJitter(float jitterAmount, float jitterSpeed)
    {
        float jitterX = Mathf.PerlinNoise(Time.time * jitterSpeed, 0) * 2 - 1;
        float jitterY = Mathf.PerlinNoise(0, Time.time * jitterSpeed) * 2 - 1;

        Vector3 jitterOffset = new Vector3(jitterX, jitterY, 0) * jitterAmount;
        comboText.transform.localPosition = originalPosition + jitterOffset;
    }
}
