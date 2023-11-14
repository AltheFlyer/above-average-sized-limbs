using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Collider2D hitBox;
    public LayerMask hitMask;
    void Start()
    {
        hitBox = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsMaskMatched(hitMask, col.gameObject))
        {
            Debug.Log("Hit Enemy");
            GlobalEventHandle.instance?.onHit.Raise(new HitData());


            col.gameObject.GetComponent<Damageable>().TakeDamage(1);
        }

    }

    public virtual bool IsMaskMatched(LayerMask mask, GameObject target)
    {
        return ((mask.value & (1 << target.layer)) > 0);
    }
}
