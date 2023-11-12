using UnityEngine;

/// <summary>
/// Singleton class that acts as a way to reference important game events.
/// Implements MonoBehaviour so you can simply set the important events 
/// in the scene(s) needed, and have other scripts use them without 
/// any additionl parameter passing.
/// </summary>
public class GlobalEventHandle : MonoBehaviour
{

    // Add important events below!
    public OnHitEvent onHit;
    public PreAttackEvent preAttack;

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