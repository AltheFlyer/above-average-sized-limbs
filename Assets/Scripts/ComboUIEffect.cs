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

    public void ShowComboEffect()
    {
        currentComboText.SetActive(true);
        //Debug.Log("Combo on");
        float colorRange = Random.Range(0.4f, 1.0f);
        currentComboText.GetComponent<MeshRenderer>().material.color = new Color(colorRange, colorRange, colorRange, colorRange);
        currentComboText.GetComponent<TextMesh>().text = "Combo: " + comboCount.Value.ToString();
        Invoke("HideComboText", 1f);

    }

    void HideComboText()
    {
        currentComboText.SetActive(false);
        //Debug.Log("Combo off");
    }
}
