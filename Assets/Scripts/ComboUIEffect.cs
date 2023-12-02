using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboUIEffect : MonoBehaviour
{
    public IntVariable comboCount;
    // public GameObject comboText;
    // private GameObject currentComboText;
    //public GameObject comboText;
    public TextMeshProUGUI comboText;
    private bool isTextShaking = false;
    private float jitterAmount;
    private float jitterSpeed;
    private Vector3 originalPosition;
    void Start()
    {
        //currentComboText = Instantiate(comboText, this.transform.position, Quaternion.identity, transform);
        comboText.gameObject.SetActive(true);
        originalPosition = comboText.transform.position;
    }
    void Update()
    {

        if (isTextShaking)
        {
            float jitterX = Mathf.PerlinNoise(Time.time * jitterSpeed, 0) * 2 - 1;
            float jitterY = Mathf.PerlinNoise(0, Time.time * jitterSpeed) * 2 - 1;

            Vector3 jitterOffset = new Vector3(jitterX, jitterY, 0) * jitterAmount;
            comboText.transform.position = originalPosition + jitterOffset;
        }

        ShowComboEffect();
    }

    public void ShowComboEffect()
    {
        Color comboTextColor = Color.white;

        if (comboCount.Value <= 1)
        {
            comboTextColor = Color.white;
            isTextShaking = false;
            jitterAmount = 0f;
            jitterSpeed = 0f;
        }
        else if (comboCount.Value <= 3 && comboCount.Value > 1)
        {
            comboTextColor = Color.yellow;
            isTextShaking = true;
            jitterAmount = 4f;
            jitterSpeed = 20f;
        }
        else
        {
            comboTextColor = Color.red;
            isTextShaking = true;
            jitterAmount = 6f;
            jitterSpeed = 30f;
        }


        comboText.color = comboTextColor;
        comboText.text = "Combo: " + comboCount.Value.ToString();
    }

    void ApplyJitter()
    {
        float jitterX = Mathf.PerlinNoise(Time.time * jitterSpeed, 0) * 2 - 1;
        float jitterY = Mathf.PerlinNoise(0, Time.time * jitterSpeed) * 2 - 1;

        Vector3 jitterOffset = new Vector3(jitterX, jitterY, 0) * jitterAmount;
        comboText.transform.localPosition = originalPosition + jitterOffset;
    }
}

//     private void RotateText(GameObject textObject, float angle, float maxAngle)
//     {
//         //textObject.transform.Rotate(Vector3.forward, angle);
//         while (textObject.transform.rotation.z <= maxAngle)
//         {
//             textObject.transform.Rotate(Vector3.forward, angle);
//         }
//     }
// }
