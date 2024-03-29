using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class AttackCollider : MonoBehaviour
{
    [Header("Target")]
    public LayerMask targetMask;
    public LayerMask destroyMask;

    [Header("Damage")]
    public int damage;

    [Header("Self")]
    public float lifetime = 10f;
    protected float time;
    public enum ExpireMode
    {
        Destroy,
        Disable,
        Nothing
    };
    public ExpireMode expireMode;
    public UnityEvent onExpiredEvent;
    public enum HitMode
    {
        Destroy,
        Disable,
        Nothing
    };
    public HitMode hitMode;
    public UnityEvent onHitEvent;

    protected virtual void OnEnable()
    {
        time = 0;
    }

    protected virtual void FixedUpdate()
    {
        if (expireMode != ExpireMode.Nothing)
        {
            time += Time.fixedDeltaTime;

            if (time > lifetime) OnExpired();
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        // Check if hit destroyMask
        if (IsMaskMatched(destroyMask, collision.gameObject))
        {
            OnHit();
            return;
        }

        // Check if the collision is with a Damageable
        Damageable d;
        collision.gameObject.TryGetComponent<Damageable>(out d);
        if (d == null) return;

        // Check if layer mask matches
        if (!IsMaskMatched(targetMask, collision.gameObject)) return;

        // Apply damage
        bool hit = d.TakeDamage(damage);

        if (hit) OnHit();
    }

    public virtual bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }

    public virtual void OnHit()
    {
        onHitEvent.Invoke();
        switch (hitMode)
        {
            case HitMode.Destroy:
                Destroy(this.gameObject);
                break;
            case HitMode.Disable:
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public virtual void OnExpired()
    {
        onExpiredEvent.Invoke();
        switch (expireMode)
        {
            case ExpireMode.Destroy:
                Destroy(this.gameObject);
                break;
            case ExpireMode.Disable:
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
