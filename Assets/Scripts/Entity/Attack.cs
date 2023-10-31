using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
    [Header("Target Settings")]
    public LayerMask targetMask;

    [Header("Damage Settings")]
    public int damage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a Damageable
        Damageable d;
        collision.gameObject.TryGetComponent<Damageable>(out d);
        if (d == null) return;

        // Apply damage
        d.TakeDamage(damage);
    }
}
