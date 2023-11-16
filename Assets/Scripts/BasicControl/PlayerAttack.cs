using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Collider2D hitBox;
    public LayerMask hitMask;

    public PlayerVariables playerVars;
    private float comboTimeLimit;
    public IntVariable comboCount;
    public FloatVariable comboTimer;

    void Start()
    {
        hitBox = GetComponent<Collider2D>();

        comboTimeLimit = playerVars.comboTimeLimit;
        comboCount.SetValue(0);
        ResetComboTimer();
    }

    void Update()
    {
        // Update the combo timer
        comboTimer.ApplyChange(-Time.deltaTime);

        // Check if the combo timer has reached zero
        if (comboTimer.Value <= 0)
        {
            // Reset the combo if the timer has elapsed
            comboCount.SetValue(0);
            // Reset the timer
            ResetComboTimer();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsMaskMatched(hitMask, col.gameObject))
        {
            comboCount.ApplyChange(1);
            Debug.Log("Combo: " + comboCount.Value);
            ResetComboTimer();
            GlobalEventHandle.instance?.onHit.Raise(new HitData());

            col.gameObject.GetComponent<Damageable>().TakeDamage(1);

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
