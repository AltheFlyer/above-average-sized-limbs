using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerVariables playerVars;
    public IntVariable currentHealth;

    void Start()
    {
        currentHealth.Value = playerVars.maxHealth;

    }

    void Update()
    {

    }

    public void DamagePlayer(int damage)
    {
        currentHealth.ApplyChange(damage);
    }
}
