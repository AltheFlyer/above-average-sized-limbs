using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class EnemyIntern : Enemy
{
    [Header("Enemy Intern")]
    public float speed = 7;
    public Transform exclaimationEffectPos;
    public GameObject exclaimationEffect;
    public GameObject deathEffect;
    public ScaleOpacityCurve spriteDeathFadeSOC;

    public Collider2D attackCollider;

    GameObject player;

    Rigidbody2D rb;
    Collider2D col;
    Animator animator;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GlobalEventHandle.instance.enemySpawn.Raise(new EnemySpawnData(gameObject));

        base.Start();

        StartCoroutine(StateStartIE());
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(1);
        }
        if (spriteDeathFadeSOC == null || spriteDeathFadeSOC.enabled)
        {
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator StateStartIE()
    {
        // Start

        if (player == null) yield break;

        // Show exclamation mark
        if (exclaimationEffect != null)
            Instantiate(exclaimationEffect, exclaimationEffectPos.position, Quaternion.identity, transform);

        // wait a bit
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(StateFollowIE());
    }

    IEnumerator StateFollowIE()
    {
        rb.velocity = Vector2.zero;

        Vector2 dir;

        while (true)
        {
            // Dampen knockback
            rb.velocity *= Mathf.Pow(0.1f, Time.deltaTime);
            if (player == null)
            {
                yield return null;
                continue;
            }
            // aim at player
            dir = (player.transform.position - transform.position).normalized;
            animator.SetFloat("xDir", dir.x);

            // rotate attack path
            //rb.velocity =
            transform.position += new Vector3(dir.x, dir.y, 0) * speed * Time.deltaTime;

            yield return null;
        }
    }

    public override void OnDead()
    {
        base.OnDead();

        attackCollider.enabled = false;
        GlobalEventHandle.instance.enemyDeath.Raise(new EnemyDeathData(gameObject));

        StopAllCoroutines();
        StartCoroutine(OnDeadIE());
    }

    IEnumerator OnDeadIE()
    {
        rb.velocity = Vector2.zero;
        col.enabled = false;

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        spriteDeathFadeSOC.enabled = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
