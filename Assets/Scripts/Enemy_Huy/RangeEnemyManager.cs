using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float shotCoolDown;
    public float startShotCoolDown;
    private GameObject player;
    public float fireForce;
    private int buffer;

    void Start()
    {
        shotCoolDown = startShotCoolDown;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        SetEnemyDirection();
        if (shotCoolDown <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(buffer*firePoint.right * fireForce,ForceMode2D.Impulse);
            shotCoolDown = startShotCoolDown;
        } else {
            shotCoolDown -= Time.deltaTime;
        }
    }

    private void SetEnemyDirection()
    {
        if (player.transform.position.x >= this.transform.position.x)
        {
            buffer = 1;
            //enemyAnimator.SetBool("faceRight", true);
            GetComponent<SpriteRenderer>().flipX = true;
        } else {
            buffer = -1;
            GetComponent<SpriteRenderer>().flipX = false;
            //enemyAnimator.SetBool("faceRight", false);
        }
    } 
    
}
