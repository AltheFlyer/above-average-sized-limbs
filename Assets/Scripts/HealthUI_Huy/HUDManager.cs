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
        // TODO: change to use player damage event listener
        // healthText.text = "Health: " + GameManager.instance.currentHealth.Value;
    }
}
