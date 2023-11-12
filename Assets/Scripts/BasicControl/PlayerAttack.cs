using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Collider2D hitBox;
    // Start is called before the first frame update
    void Start()
    {
        hitBox = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GlobalEventHandle.instance?.onHit.Raise(new HitData());
            col.gameObject.GetComponent<Damageable>().TakeDamage(1);
        }

    }
}
