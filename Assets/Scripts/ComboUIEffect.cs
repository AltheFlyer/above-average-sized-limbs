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
        float colorRange = Random.Range(0.0f, 1.0f);
        currentComboText.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 1, 1);
        currentComboText.GetComponent<TextMesh>().text = "Combo: " + comboCount.Value.ToString();
    }

}
