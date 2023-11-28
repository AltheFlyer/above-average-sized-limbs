using UnityEngine;

/// <summary>
/// Singleton class that acts as a way to reference important game events.
/// Implements MonoBehaviour so you can simply set the important events 
/// in the scene(s) needed, and have other scripts use them without 
/// any additional parameter passing.
///
/// Also acts as an intermediate event handler, converting general events (TakeDamage) 
/// to specific ones (PlayerTookDamage)
/// </summary>
public class GlobalEventHandle : MonoBehaviour
{

    // Add important events below!
    [Header("Event Handles")]
    public OnHitEvent onHit;
    public PreAttackEvent preAttack;

    public TakeDamageEvent takeDamage;

    public SimpleEvent playerStatChange;

    public EnemySpawnEvent enemySpawn;
    public EnemyDeathEvent enemyDeath;

    // Events that this object specifically invokes
    [Header("Derived Events")]
    public TakeDamageEvent playerTookDamage;

    public PlayerDeathEvent playerDeath;

    private static GlobalEventHandle _instance;

    public static GlobalEventHandle instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = this;
        Debug.Log("Setting instance");
    }

    public void TakeDamageHandler(DamageData data)
    {
        Debug.Log($"Damage event happened to: {data.target}");
        if (data.target.GetComponent<PlayerManager>() != null)
        {
            playerTookDamage.Raise(data);
            if (data.postDamageHealth <= 0)
            {
                Debug.Log("player ded rip");
                playerDeath.Raise(new PlayerDeathData(data.target.gameObject));
            }
        }
    }

    // Debug methods
    public void LogOnHit(HitData data)
    {
        Debug.Log($"OnHit Event: {data}");
    }

    public void LogPreAttack(AttackData data)
    {
        Debug.Log($"PreAttack Event: {data}");
    }
}