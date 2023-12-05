using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Singleton<BossHealth>
{
    public GameObject managerHealthBar;
    public GameObject CEOHealthBar;
    public Slider managerSlider;
    public Slider CEOSlider;

    public void Start()
    {
        managerSlider.maxValue = 1;
        CEOSlider.maxValue = 1;
        managerSlider.minValue = 0;
        CEOSlider.minValue = 0;
        managerSlider.value = 0;
        CEOSlider.value = 0;
    }

    public void SetManagerHPUI(float hpRatio)
    {
        if (hpRatio == 0)
        {
            managerHealthBar.SetActive(false);
        }
        else if (hpRatio == 1 && managerSlider.value == 0)
        {
            managerHealthBar.SetActive(true);
        }
        managerSlider.value = hpRatio;
    }
    public void SetCEOHPUI(float hpRatio)
    {
        if (hpRatio == 0)
        {
            CEOHealthBar.SetActive(false);
        }
        else if (hpRatio == 1 && CEOSlider.value == 0)
        {
            CEOHealthBar.SetActive(true);
        }
        CEOSlider.value = hpRatio;
    }
}
