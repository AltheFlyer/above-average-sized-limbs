using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerAttack : MonoBehaviour
{
    private Collider2D hitBox;
    public LayerMask hitMask;

    //Combo
    public PlayerVariables playerVars;
    private float comboTimeLimit;
    public IntVariable comboCount;
    public FloatVariable comboTimer;

    public GameObject comboTextPrefab;

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
            //For whoever's gonna modify this, the comboCount, showComboEffect and ResetTimer
            //doesn't work when put below the GlobalEventHandle.
            comboCount.ApplyChange(1);
            ShowComboEffect();
            ResetComboTimer();


            GlobalEventHandle.instance?.onHit.Raise(new HitData());
            col.gameObject.GetComponent<Damageable>().TakeDamage(1);

        }
    }

    private void ShowComboEffect()
    {
        GameObject comboText = Instantiate(comboTextPrefab, this.transform.position, Quaternion.identity, transform);
        comboText.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        comboText.GetComponent<TextMesh>().text = "Combo: " + comboCount.Value.ToString();
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
