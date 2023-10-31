using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainer;
    public GameConstants gameConstants;
    private float fillValue;

    void Update()
    {
        fillValue = (float)GameManager.instance.currentHealth.Value;
        fillValue = fillValue/gameConstants.maxHealth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
        
    }
}
