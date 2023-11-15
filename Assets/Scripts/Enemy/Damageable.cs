using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    protected int maxHP;
    [SerializeField]
    protected int hp;
    protected float invincibleTime;
    [SerializeField]
    protected float onDamagedInvincibleTime;

    [Header("Effects")]
    public UnityEvent onDamage;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        hp = maxHP;
    }

    protected virtual void Update()
    {
        invincibleTime -= Time.deltaTime;
    }

    public virtual bool IsInvincible()
    {
        return invincibleTime > 0;
    }

    ///<summary>Call by OnCollision2D of the attacking gameobject after checking layer or type of Damageable. Will return false if the target is invincible</summary>
    public virtual bool TakeDamage(int damage, Action onKillShot = null)
    {
        // check if can take damage
        if (IsInvincible()) return false;
        if (hp <= 0) return false;

        // check if is a kill shot
        if (hp - damage <= 0 && onKillShot != null) onKillShot();

        // apply damage
        ApplyHP(-damage);

        // Emit events
        onDamage.Invoke();
        GlobalEventHandle.instance.takeDamage.Raise(new DamageData(
            this, damage, hp
        ));

        // set damage invisible time
        if (onDamagedInvincibleTime > 0)
        {
            invincibleTime = onDamagedInvincibleTime;
        }

        return true;
    }

    public virtual void ApplyHP(int change)
    {
        hp += change;
        hp = Math.Min(hp, maxHP);

        if (hp <= 0) OnDead();
    }

    public virtual void OnDead()
    {
        Destroy(this.gameObject);
    }

    // (Yes, it's just a wrapper around ApplyHP, but maybe we can put events in here idk)
    public virtual void Heal(int amount)
    {
        ApplyHP(amount);
    }

    public virtual void SetMaxHP(int newMax)
    {
        maxHP = newMax;
    }

    public virtual int GetHP()
    {
        return hp;
    }

    public virtual void TryPlaySFX(string name)
    {
        SFXManager.TryPlaySFX(name, this.gameObject);
    }
}
