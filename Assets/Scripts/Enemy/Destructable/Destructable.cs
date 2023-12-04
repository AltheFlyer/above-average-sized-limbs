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

    string[] damagedSounds = new string[] { "break2", "break3", "break4", "break5" };

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        explodable = GetComponent<Explodable>();
        spriteFlicker = GetComponent<SpriteFlicker>();

        onDamage.AddListener(spriteFlicker.Flicker);
        onDamage.AddListener(() => SFXManager.TryPlaySFX(damagedSounds, gameObject));
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnDead()
    {
        if (explodable != null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) SFXManager.TryPlaySFX("break1", p);
            CameraShake.Shake();
            explodable.explode();
        }
    }

    void OnValidate()
    {
        if (maxHP < 1) maxHP = 4;
        if (hp < 1) hp = 4;

        if (gameObject.layer != LayerMask.NameToLayer("Destructable"))
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
                explodable.extraPoints = 2;
                explodable.fragmentLayer = "NoCollision";
                explodable.sortingLayerName = "Top Effects";
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

        // if (rigidbody && explodable && spriteFlicker && transform.childCount == 0)
        // {
        //     explodable.fragmentInEditor();
        // }

        if (damageable == null)
            damageable = GetComponent<Damageable>();
    }
}
