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
    private int comboThreshold1;
    private int comboThreshold2;

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
        comboThreshold1 = playerVars.comboThreshold1;
        comboThreshold2 = playerVars.comboThreshold2;

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
        //ComboStatCheck();

    }

    void ComboStatCheck()
    {
        if (comboCount.Value >= comboThreshold1 && !damageIncreaseByCombo)
        {
            currentAttackDamage += 1;
            damageIncreaseByCombo = true;
        }
        else if (comboCount.Value < comboThreshold1)
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
                float knockbackForce;
                if (comboCount.Value <= comboThreshold1)
                {
                    knockbackForce = Mathf.Min(5 * attackDamage * data.attackSizeMultiplier, 10.0f);
                }
                else if (comboCount.Value > comboThreshold1 && comboCount.Value <= comboThreshold2)
                {
                    knockbackForce = Mathf.Min(15 * attackDamage * data.attackSizeMultiplier, 20.0f);
                }
                else
                {
                    knockbackForce = Mathf.Min(25 * attackDamage * data.attackSizeMultiplier, 30.0f);
                }

                col.attachedRigidbody.AddForce((col.transform.position - transform.parent.position).normalized * knockbackForce, ForceMode2D.Impulse);

                //col.attachedRigidbody.AddForce((col.transform.position - transform.parent.position).normalized *
                //Mathf.Min(5 * attackDamage * data.attackSizeMultiplier, 10.0f), ForceMode2D.Impulse);

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
