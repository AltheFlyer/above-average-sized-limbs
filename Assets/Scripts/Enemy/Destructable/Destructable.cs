using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Destructable : Damageable
{
    new Rigidbody2D rigidbody;
    Explodable explodable;
    SpriteFlicker spriteFlicker;

    public Damageable damageable;

    protected override void Awake()
    {
        base.Awake();

        onDamage.AddListener(spriteFlicker.Flicker);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnDead()
    {
        if (explodable != null)
        {
            explodable.explode();
        }
    }

    void OnValidate()
    {
        if (maxHP < 1) maxHP = 1;
        if (hp < 1) hp = 1;

        gameObject.layer = LayerMask.NameToLayer("Destructable");

        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody2D>();
                rigidbody.mass = 99999;
                rigidbody.gravityScale = 0;
            }
        }

        if (explodable == null)
        {
            explodable = GetComponent<Explodable>();
            if (explodable == null)
            {
                explodable = gameObject.AddComponent<Explodable>();
                explodable.shatterType = Explodable.ShatterType.Voronoi;
                explodable.extraPoints = 3;
                explodable.fragmentLayer = "NoCollision";
                explodable.sortingLayerName = "Background";
                explodable.orderInLayer = 5;
            }
        }

        if (spriteFlicker == null)
        {
            spriteFlicker = GetComponent<SpriteFlicker>();
            if (spriteFlicker == null)
            {
                spriteFlicker = gameObject.AddComponent<SpriteFlicker>();
                spriteFlicker.spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        if (rigidbody && explodable && spriteFlicker && transform.childCount == 0)
        {
            explodable.fragmentInEditor();
        }

        if (damageable == null)
            damageable = GetComponent<Damageable>();
    }
}
