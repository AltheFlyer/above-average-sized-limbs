using UnityEngine;

[CreateAssetMenu(menuName = "Event/OnHit")]
public class OnHitEvent : GameEvent<HitData>
{

}

public class HitData
{
    // UNKNOWN TYPE source;
    // UNKNOWN TYPE the attack that caused the hit
    Damageable target;
}