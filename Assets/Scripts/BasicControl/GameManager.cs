using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameConstants gameConstants;
    public IntVariable currentHealth;
    
    void Start()
    {
        currentHealth.Value = gameConstants.maxHealth;
        
    }

    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        currentHealth.ApplyChange(damage);
    }
}
