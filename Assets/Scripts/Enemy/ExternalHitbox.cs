using System;
using UnityEngine;
using UnityEngine.Events;

public class ExternalHitbox : Damageable
{
    public Damageable entityToHurt;

    public override bool TakeDamage(float damage, Action onKillShot = null)
    {
        return entityToHurt.TakeDamage(damage, onKillShot);
    }
}