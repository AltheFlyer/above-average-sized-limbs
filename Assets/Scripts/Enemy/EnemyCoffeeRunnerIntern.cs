using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

// (Peter Griffin Enemy)
public class EnemyCoffeeRunnerIntern : Enemy
{
    [Header("Enemy Coffee Runner Intern")]
    public LayerMask wallMask;
    public float attackSpeed = 7;
    public float prepareTime = 3;
    public float attackDelay = 0.3f;
    public float stunTime = 3;
    [Space]
    public Transform attackPath;
    public ParticleSystem dustEffect;
    [Space]
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
        if (GlobalEventHandle.instance != null) GlobalEventHandle.instance.enemySpawn.Raise(new EnemySpawnData(gameObject));

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
    }

    IEnumerator StateStartIE()
    {
        // Start

        if (player == null) yield break;

        // Show exclamation mark
        if (exclaimationEffect != null)
            Instantiate(exclaimationEffect, exclaimationEffectPos.position, Quaternion.identity, transform);

        // hide attack path
        attackPath.gameObject.SetActive(false);

        // wait a bit
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(StatePrepareIE());
    }

    IEnumerator StatePrepareIE()
    {
        rb.velocity = Vector2.zero;

        // show attack path
        attackPath.gameObject.SetActive(true);

        float _prepareTime = prepareTime;

        float angle = 0;

        animator.SetTrigger("startRolling");

        while (_prepareTime > 0)
        {
            // aim at player
            angle = AngPosUtil.GetAngle(transform.position, player.transform.position);

            // rotate attack path
            attackPath.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
            _prepareTime -= Time.deltaTime;
        }

        yield return new WaitForSeconds(attackDelay);

        // charge at player
        StartCoroutine(StateAttackIE(angle));
    }

    IEnumerator StateAttackIE(float angle)
    {
        animator.SetTrigger("rolling");

        rb.velocity = AngPosUtil.GetAngularPos(angle, attackSpeed);

        yield return new WaitForSeconds(0.3f);

        yield return WaitTilHitWall();
        animator.SetTrigger("stopRolling");

        StartCoroutine(StateStunnedIE());
    }

    IEnumerator StateStunnedIE()
    {
        // Stunned
        attackPath.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(stunTime);

        StartCoroutine(StatePrepareIE());
    }

    IEnumerator WaitTilHitWall()
    {
        while (true)
        {
            if (col.IsTouchingLayers(wallMask))
            {
                yield break;
            }
            yield return null;
        }
        animator.SetTrigger("startRolling");
    }

    public override void OnDead()
    {
        base.OnDead();

        attackCollider.enabled = false;
        if (GlobalEventHandle.instance != null) GlobalEventHandle.instance.enemyDeath.Raise(new EnemyDeathData(gameObject));

        StopAllCoroutines();
        StartCoroutine(OnDeadIE());
    }

    IEnumerator OnDeadIE()
    {
        rb.velocity = Vector2.zero;
        attackPath.gameObject.SetActive(false);
        dustEffect.Stop();
        col.enabled = false;

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        spriteDeathFadeSOC.enabled = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
