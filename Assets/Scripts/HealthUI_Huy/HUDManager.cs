using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void Update()
    {
        healthText.text = "Health: " + GameManager.instance.currentHealth.Value;
    }
}
