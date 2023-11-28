using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUIEffect : MonoBehaviour
{
    public IntVariable comboCount;
    public GameObject comboTextPrefab;
    private GameObject currentComboText;

    void Start()
    {
        //Combo Effect
        currentComboText = Instantiate(comboTextPrefab, this.transform.position, Quaternion.identity, transform);
        currentComboText.SetActive(false);
    }

    void Update()
    {
        if (comboCount.Value == 0)
        {
            currentComboText.SetActive(false);
        }
    }

    public void ShowComboEffect()
    {
        currentComboText.SetActive(true);
        // float saturation = Random.Range(0.7f, 1.0f);
        // float brightness = Random.Range(0.7f, 1.0f);
        Color randomColor = Color.HSVToRGB(Random.value, 1, 1);
        currentComboText.GetComponent<MeshRenderer>().material.color = randomColor;
        currentComboText.GetComponent<TextMesh>().text = "Combo: " + comboCount.Value.ToString();
    }

}
