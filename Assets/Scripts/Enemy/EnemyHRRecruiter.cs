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
    public Transform[] summonPos;
    public ParticleSystem[] summonParticle;

    public GameObject hiredEffectPrefab;
    public GameObject summonPrefab;
    [Space]
    public Transform facePalmEffectPos;
    public GameObject facePalmEffectPrefab;

    public Collider2D attackCollider;

    List<GameObject> summons = new List<GameObject>();

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
        if (GlobalEventHandle.instance != null)
            GlobalEventHandle.instance.enemySpawn.Raise(new EnemySpawnData(gameObject));

        base.Start();

        // set default
        summonStateParticle.Stop();
        foreach (ParticleSystem ps in summonParticle)
        {
            ps.Stop();
        }

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
        foreach (ParticleSystem ps in summonParticle)
        {
            if (ps != null)
            {
                ps.Play();
            }
        }
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
        summons.Clear();
        foreach (Transform t in summonPos)
        {
            Instantiate(hiredEffectPrefab, t.position, Quaternion.identity, transform);
            summons.Add(Instantiate(summonPrefab, t.position, Quaternion.identity));
        }

        foreach (ParticleSystem ps in summonParticle)
        {
            if (ps != null)
            {
                ps.Stop();
            }
        }
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
            bool stillAlive = false;
            foreach (GameObject go in summons)
            {
                if (go != null)
                {
                    stillAlive = true;
                }
            }
            if (!stillAlive)
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
        if (GlobalEventHandle.instance != null)
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
