using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public float bulletSpeed;
    void Update()
    {
        //transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
        
    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.instance.DamagePlayer(-1);

        }
        Destroy(gameObject);
    }
}
