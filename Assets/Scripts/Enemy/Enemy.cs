using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    public override void OnDead()
    {
        SFXManager.TryPlaySFX("fired1", gameObject);
    }
}
