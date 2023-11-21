using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBounce : MonoBehaviour
{
    public int bounceCount = 3;
    public LayerMask bounceMask;
    public AttackCollider attackCollider;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (IsMaskMatched(bounceMask, other.gameObject))
        {
            bounceCount--;
            if (bounceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }
}
