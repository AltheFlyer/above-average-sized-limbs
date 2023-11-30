using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class EnemyVP : Enemy
{
    [Header("Enemy Intern")]
    public Transform exclaimationEffectPos;
    public GameObject exclaimationEffect;
    public GameObject deathEffect;
    public ScaleOpacityCurve spriteDeathFadeSOC;

    [Header("Follow Phase")]
    public float followSpeed = 7;

    [Header("Punch Phase")]
    public float punchRadiusThreshold;
    public float punchSpeed = 14;
    public float punchTime = 0.3f;
    public float punchDelay = 1f;
    public float postPunchDelay = 2f;
    public GameObject attackTransform;
    public Transform attackColliderTransform;
    private Animator attackAnim;
    private ScaleOpacityCurve attackSOC;
    private SpriteRenderer attackSR;
    public Collider2D attackCollider;
    public CamShaker camShaker;

    private string[] attackSFX = { "punch1", "punch2", "punch3" };

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

        attackAnim = attackTransform.GetComponent<Animator>();
        attackSOC = attackTransform.GetComponent<ScaleOpacityCurve>();
        attackSR = attackTransform.GetComponent<SpriteRenderer>();
        attackTransform.SetActive(false);
        attackColliderTransform.gameObject.SetActive(false);
        attackCollider.enabled = false;

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
            if (player == null)
            {
                yield return null;
                continue;
            }
            // aim at player
            dir = (player.transform.position - transform.position).normalized;
            animator.SetFloat("xDir", dir.x);

            rb.velocity = dir * followSpeed;

            // check if player is close enough
            float sqrDst = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDst < punchRadiusThreshold * punchRadiusThreshold)
            {
                // if close enough, start punch phase
                StartCoroutine(StatePunchIE());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator StatePunchIE()
    {
        rb.velocity = Vector2.zero;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        animator.SetFloat("xDir", dir.x);

        // show pre attack
        attackTransform.SetActive(true);
        attackColliderTransform.gameObject.SetActive(true);
        attackSR.color = new Color(attackSR.color.r, attackSR.color.g, attackSR.color.b, 1);
        attackSOC.enabled = false;
        attackCollider.enabled = false;
        attackAnim.SetTrigger("Pre");
        attackTransform.transform.eulerAngles = new Vector3(0, 0, AngPosUtil.GetAngle(Vector2.zero, dir));
        attackColliderTransform.eulerAngles = new Vector3(0, 0, AngPosUtil.GetAngle(Vector2.zero, dir));
        attackSR.flipY = dir.x < 0;

        // delay before punching
        yield return new WaitForSeconds(punchDelay);

        // show attack
        attackTransform.SetActive(true);
        attackColliderTransform.gameObject.SetActive(true);
        attackSR.color = new Color(attackSR.color.r, attackSR.color.g, attackSR.color.b, 1);
        attackSOC.enabled = false;
        attackCollider.enabled = true;
        attackAnim.SetTrigger("Attack");
        camShaker.Activate();
        SFXManager.TryPlaySFX(attackSFX, gameObject);

        float _time = punchTime;

        while (_time > 0)
        {
            // rotate attack path
            rb.velocity = dir * punchSpeed;

            _time -= Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;

        // hide attack
        attackTransform.SetActive(true);
        attackColliderTransform.gameObject.SetActive(true);
        attackSR.color = new Color(attackSR.color.r, attackSR.color.g, attackSR.color.b, 1);
        attackSOC.enabled = true;
        attackCollider.enabled = false;

        yield return new WaitForSeconds(postPunchDelay);

        // disable attack
        attackTransform.SetActive(false);
        attackColliderTransform.gameObject.SetActive(false);
        attackSOC.enabled = false;

        StartCoroutine(StateFollowIE());
    }

    public override void OnDead()
    {
        base.OnDead();

        if (GlobalEventHandle.instance != null) GlobalEventHandle.instance.enemyDeath.Raise(new EnemyDeathData(gameObject));

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, punchRadiusThreshold);
    }
}
