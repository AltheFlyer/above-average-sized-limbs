using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainerPrefab;
    public GameObject filledHeartPrefab;

    public PlayerVariables playerVars;

    public int spacing;

    private float fillValue;

    void Start()
    {
        UpdateDisplayAttempt(null);
    }

    public void SetDisplay(int currentHealth, int maxHealth)
    {
        Vector3 uiPosition = transform.position;

        // Derender old children (inefficient, but probably ok)
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        // Render heart containers
        for (int i = 0; i < maxHealth / 2; ++i)
        {
            Instantiate(heartContainerPrefab, uiPosition + new Vector3(spacing * i, 0, 0), Quaternion.identity, transform);
        }

        // Render full hearts
        for (int i = 0; i < currentHealth / 2; ++i)
        {
            Instantiate(filledHeartPrefab, uiPosition + new Vector3(spacing * i, 0, 0), Quaternion.identity, transform);
        }

        // Render half heart if needed
        if (currentHealth % 2 == 1)
        {
            GameObject halfHeart = Instantiate(filledHeartPrefab, uiPosition + new Vector3(spacing * (currentHealth / 2), 0, 0), Quaternion.identity, transform);
            halfHeart.GetComponent<Image>().fillAmount = 0.5f;
        }
    }

    public void UpdateDisplay(DamageData data)
    {
        int currentHealth = (int)data.postDamageHealth;
        int maxHealth = playerVars.maxHealth;

        SetDisplay(currentHealth, maxHealth);
    }

    // Jank, tries to update display without being given anything
    public void UpdateDisplayAttempt(object o)
    {
        int? currentHealth = FindObjectOfType<PlayerManager>()?.GetComponent<Damageable>()?.GetHP();
        if (currentHealth == null)
        {
            return;
        }
        int maxHealth = playerVars.maxHealth;

        SetDisplay((int)currentHealth, maxHealth);
    }
}
