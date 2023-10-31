using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderAttack : MonoBehaviour
{
    [Header("Target")]
    public LayerMask targetMask;

    [Header("Damage")]
    public int damage;

    [Header("Self")]
    public float lifetime = 10f;

    protected void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;

        if (lifetime < 0) OnExpired();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a Damageable
        Damageable d;
        collision.gameObject.TryGetComponent<Damageable>(out d);
        if (d == null) return;

        // Check if layer mask matches
        if (!IsMaskMatched(targetMask, collision.gameObject)) return;

        // Apply damage
        d.TakeDamage(damage);
    }

    public virtual bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }

    protected virtual void OnHit()
    {
        Destroy(this.gameObject);
    }

    protected virtual void OnExpired()
    {
        Destroy(this.gameObject);
    }
}
