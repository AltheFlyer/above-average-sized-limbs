using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineCaster))]
public class LineAttackCaster : MonoBehaviour
{
    [Header("Target")]
    public ContactFilter2D targetContactFilter2D;

    [Header("Damage")]
    public int damage;

    [Header("Self")]
    public float width;

    public BoxCollider2D col;
    private LineCaster lineCaster;

    protected virtual void Awake()
    {
        lineCaster = GetComponent<LineCaster>();
    }

    public virtual void Disable()
    {
        col.enabled = false;
        this.enabled = false;
    }

    public virtual void Enable()
    {
        col.enabled = true;
        this.enabled = true;
    }

    protected virtual void FixedUpdate()
    {
        UpdateCollider();
        CheckCollision();
    }

    protected virtual void UpdateCollider()
    {
        col.size = new Vector2(lineCaster.GetDistance(), width);
        col.offset = new Vector2(lineCaster.GetDistance() / 2, 0);
    }

    protected virtual void CheckCollision()
    {
        Collider2D[] colliders = new Collider2D[10];
        int count = Physics2D.OverlapCollider(col, targetContactFilter2D, colliders);

        foreach (var c in colliders)
        {
            if (c == null) continue;
            OnCollisionUpdate(c);
        }
    }

    protected virtual void OnCollisionUpdate(Collider2D c)
    {
        // Check if the collision is with a Damageable
        Damageable d;
        c.gameObject.TryGetComponent<Damageable>(out d);
        if (d == null) return;

        // Apply damage
        bool hit = d.TakeDamage(damage);

        if (hit) OnHit();
    }

    protected virtual void OnHit()
    {

    }
}
