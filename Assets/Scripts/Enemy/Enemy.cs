using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Damageable
{
    private Rigidbody2D rb;

    [Space]
    [Header("Enemy Anti Clustering (require rigidbody2D)")]
    public bool antiClustering = false;
    public float antiClusteringRadius = 0.75f;
    static List<Enemy> antiClusteringEnemies = new List<Enemy>();
    readonly static float antiClusteringForce = 3;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            antiClustering = false;
        if (antiClustering)
        {
            antiClusteringEnemies.Add(this);
        }
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        if (antiClusteringEnemies.Contains(this))
        {
            antiClusteringEnemies.Remove(this);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (antiClustering)
        {
            foreach (Enemy enemy in antiClusteringEnemies)
            {
                if (enemy == this) continue;
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < antiClusteringRadius * 2)
                {
                    Vector2 dir = (transform.position - enemy.transform.position).normalized;
                    rb.AddForce(dir * (antiClusteringRadius * 2 - dist) * antiClusteringForce);
                }
            }
        }
    }

    public override void OnDead()
    {
        SFXManager.TryPlaySFX("fired1", gameObject);
    }

    public virtual void OnDrawGizmos()
    {
        if (antiClustering)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, antiClusteringRadius);
        }
    }
}
