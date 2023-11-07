using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class BossManager : Damageable
{
    [Header("Enemy Intern")]
    public GameObject deathEffectPrefab;
    public ScaleOpacityCurve spriteDeathFadeSOC;
    [Space]
    public float startTime = 0.5f;
    [Space]
    public float followSpeed = 3f;
    public float followDuration = 3f;
    [Space]
    public float fireCount = 3;
    public float fireAimTime = 1f;
    public float fireDelay = 0.75f;
    public


    GameObject player;

    Rigidbody2D rb;
    Collider2D col;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

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

        // wait a bit
        yield return new WaitForSeconds(startTime);

        StartCoroutine(RandomNextStateIE());
    }

    public enum State
    {
        None = -1,
        Follow = 0,
        Fire = 1,
        Overtime = 2,
        Chase = 3
    }
    public State state = State.None;
    IEnumerator RandomNextStateIE()
    {
        rb.velocity = Vector2.zero;

        // random til get different state than old state
        int rand = Random.Range(0, 4); ;
        while (rand == (int)state)
        {
            rand = Random.Range(0, 4);
        }
        state = (State)rand;

        // do state
        switch (state)
        {
            case State.Follow:
                StartCoroutine(StateFollowIE());
                break;
            case State.Fire:
                StartCoroutine(StateFireIE());
                break;
            case State.Overtime:
                StartCoroutine(StateOvertimeIE());
                break;
            case State.Chase:
                StartCoroutine(StateChaseIE());
                break;
        }

        yield return null;
    }

    IEnumerator StateFollowIE()
    {
        rb.velocity = Vector2.zero;

        float _time = followDuration;

        Vector2 dir;

        while (_time > 0)
        {
            // aim at player
            dir = (player.transform.position - transform.position).normalized;

            // rotate attack path
            rb.velocity = dir * followSpeed;

            yield return null;
            _time -= Time.deltaTime;
        }

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateFireIE()
    {
        rb.velocity = Vector2.zero;

        yield return null;

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateOvertimeIE()
    {
        yield return null;
    }

    IEnumerator StateChaseIE()
    {
        yield return null;
    }

    public override void OnDead()
    {
        StopAllCoroutines();
        StartCoroutine(OnDeadIE());
    }

    IEnumerator OnDeadIE()
    {
        rb.velocity = Vector2.zero;
        col.enabled = false;

        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        spriteDeathFadeSOC.enabled = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
