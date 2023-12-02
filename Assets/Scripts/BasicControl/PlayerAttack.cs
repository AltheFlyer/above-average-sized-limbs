using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class PlayerAttack : MonoBehaviour
{
    private Collider2D hitBox;
    public LayerMask hitMask;

    //Combo
    public PlayerVariables playerVars;

    // Combo management
    private float comboTimeLimit;
    public IntVariable comboCount;
    public FloatVariable comboTimer;

    private float attackDamage = 1;
    private float currentAttackDamage;
    private bool damageIncreaseByCombo = false;

    // List of things that were hit (used to prevent double-hits)
    private List<Collider2D> hitList;

    private AttackData data;

    // How long the attack hitbox lingers for in seconds.
    public float lifespan;

    /// <summary>
    /// Function to be invoked by whatever instantiates the attack instance. 
    /// The passed-in AttackData is used to modify the attack we actually get.
    /// </summary>
    public void InitAttack(AttackData data)
    {
        this.attackDamage = data.damage;

        this.data = data;
    }

    void Start()
    {
        hitList = new List<Collider2D>();
        hitBox = GetComponent<Collider2D>();

        comboTimeLimit = playerVars.comboTimeLimit;

        transform.localScale *= data.attackSizeMultiplier;
        ResetComboTimer();

        currentAttackDamage = attackDamage;
    }

    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
        //ComboDamageCheck();

    }

    void ComboDamageCheck()
    {
        if (comboCount.Value >= 10 && !damageIncreaseByCombo)
        {
            currentAttackDamage += 1;
            damageIncreaseByCombo = true;
        }
        else if (comboCount.Value < 10)
        {
            currentAttackDamage = attackDamage;
            damageIncreaseByCombo = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsMaskMatched(hitMask, col.gameObject) && !hitList.Contains(col))
        {
            //For whoever's gonna modify this, the comboCount, showComboEffect and ResetTimer
            //doesn't work when put below the GlobalEventHandle.
            comboCount.ApplyChange(1);
            ResetComboTimer();

            GlobalEventHandle.instance?.onHit.Raise(new HitData());

            col.gameObject.GetComponent<Damageable>().TakeDamage(attackDamage);

            // Knockback
            if (col.GetComponent<KnockbackImmunity>() == null)
            {
                col.attachedRigidbody.AddForce((col.transform.position - transform.parent.position).normalized *
                    Mathf.Min(5 * attackDamage * data.attackSizeMultiplier, 10.0f), ForceMode2D.Impulse);
            }

            hitList.Add(col);
        }
    }

    void ResetComboTimer()
    {
        comboTimer.SetValue(comboTimeLimit);
    }

    public virtual bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }

}
