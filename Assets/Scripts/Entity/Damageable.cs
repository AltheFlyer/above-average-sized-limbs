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

    [Header("Effects")]
    public UnityEvent onDamage;

    protected virtual void Start()
    {
        hp = maxHP;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(1);
    }

    ///<summary>Call by OnCollision2D of the attacking gameobject after checking layer or type of Damageable</summary>
    public virtual bool TakeDamage(int damage)
    {
        bool killShot = false;

        if (hp - damage <= 0) killShot = true;

        ApplyHP(-damage);
        onDamage.Invoke();

        return killShot;
    }

    public virtual void ApplyHP(int change)
    {
        hp += change;

        if (hp <= 0) OnDead();
    }

    public virtual void OnDead()
    {
        Destroy(this.gameObject);
    }
}
