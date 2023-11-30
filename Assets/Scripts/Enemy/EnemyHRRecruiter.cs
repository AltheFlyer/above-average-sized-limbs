using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class EnemyHRRecruiter : Enemy
{
    [Header("Enemy Intern")]
    public Transform exclaimationEffectPos;
    public GameObject exclaimationEffectPrefab;
    public GameObject deathEffectPrefab;
    public ScaleOpacityCurve spriteDeathFadeSOC;
    [Space]
    public float startTime = 0.5f;
    public float summonTime = 2;
    public float facepalmTime = 0.5f;
    [Space]
    public ParticleSystem summonStateParticle;
    public Transform summonPos1;
    public ParticleSystem summonParticle1;
    public Transform summonPos2;
    public ParticleSystem summonParticle2;
    public GameObject hiredEffectPrefab;
    public GameObject summonPrefab;
    [Space]
    public Transform facePalmEffectPos;
    public GameObject facePalmEffectPrefab;

    public Collider2D attackCollider;

    GameObject summon1;
    GameObject summon2;

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

        // set default
        summonStateParticle.Stop();
        summonParticle1.Stop();
        summonParticle2.Stop();

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
        if (exclaimationEffectPrefab != null)
            Instantiate(exclaimationEffectPrefab, exclaimationEffectPos.position, Quaternion.identity, transform);

        // wait a bit
        yield return new WaitForSeconds(startTime);

        StartCoroutine(StateSummonIE());
    }

    IEnumerator StateSummonIE()
    {
        rb.velocity = Vector2.zero;

        // start summon
        if (summonParticle1 != null)
            summonParticle1.Play();
        if (summonParticle2 != null)
            summonParticle2.Play();
        summonStateParticle.Play();

        SFXManager.TryPlaySFX("summon1", gameObject);
        animator.SetTrigger("summon");

        // wait a bit      
        float _time = summonTime;

        while (_time > 0)
        {
            yield return null;
            _time -= Time.deltaTime;
        }

        // summon
        Instantiate(hiredEffectPrefab, summonPos1.position, Quaternion.identity, transform);
        summon1 = Instantiate(summonPrefab, summonPos1.position, Quaternion.identity);
        Instantiate(hiredEffectPrefab, summonPos2.position, Quaternion.identity, transform);
        summon2 = Instantiate(summonPrefab, summonPos2.position, Quaternion.identity);

        if (summonParticle1 != null)
            summonParticle1.Stop();
        if (summonParticle2 != null)
            summonParticle2.Stop();
        summonStateParticle.Stop();

        SFXManager.TryPlaySFX("summon1", gameObject);

        StartCoroutine(StateWaitIE());
    }

    IEnumerator StateWaitIE()
    {
        rb.velocity = Vector2.zero;

        // idle, wait for all minion to die
        while (true)
        {
            if (summon1 == null && summon2 == null)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(StateFacepalmIE());
    }

    IEnumerator StateFacepalmIE()
    {
        rb.velocity = Vector2.zero;

        // facepalm
        Instantiate(facePalmEffectPrefab, facePalmEffectPos.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(facepalmTime);

        StartCoroutine(StateSummonIE());
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

        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        spriteDeathFadeSOC.enabled = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
