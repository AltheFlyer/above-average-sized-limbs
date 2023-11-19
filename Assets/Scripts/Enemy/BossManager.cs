using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YUtil;

public class BossManager : Damageable
{
    [Header("Boss")]
    public GameObject deathEffectPrefab;
    public UnityEvent onDeadEvent;
    public ScaleOpacityCurve spriteDeathFadeSOC;
    public LayerMask wallMask;
    [Space]
    public float startTime = 0.5f;
    [Space]
    public float followSpeed = 3f;
    public float followDuration = 3f;
    [Space]
    public float fireCount = 3;
    public float fireAimTime = 1f;
    public float fireDelay = 0.3f;
    public float fireTime = 2f;
    public float fireAngleSpeed = 10f;
    public float fireTimeBetweenAttack = 0.4f;
    public float fireSmoothTime = 0.1f;
    public float fireStunTime = 3.5f;
    public GameObject fireAttack;
    public LaserAttackController laserAttackController;
    float fireCurrentVelocity;
    [Space]
    public float overtimePreDuration = 1.5f;
    public ParticleSystem overtimeParticle;
    public float overtimeAttackDuration = 5f;
    public GameObject overtimeAttack;

    [Space]
    public Transform chargeAttackPath;
    public int chargeCount = 3;
    public float chargePrepareTime = 2f;
    public float chargeAttackDelay = 0.3f;
    public float chargeSpeed = 10;
    public float chargeStunTime = 3.5f;


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
        chargeAttackPath.gameObject.SetActive(false);
        overtimeAttack.SetActive(false);

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
        Charge = 3
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
                Debug.Log("Follow");
                break;
            case State.Fire:
                StartCoroutine(StateFireIE());
                Debug.Log("Fire");
                break;
            case State.Overtime:
                StartCoroutine(StateOvertimeIE());
                Debug.Log("Overtime");
                break;
            case State.Charge:
                StartCoroutine(StateChargeIE());
                Debug.Log("Charge");
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
            animator.SetFloat("xDir", dir.x);

            // rotate attack path
            rb.velocity = dir * followSpeed;

            yield return null;
            _time -= Time.deltaTime;
        }

        animator.SetFloat("xDir", 0);
        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateFireIE()
    {
        rb.velocity = Vector2.zero;

        for (int i = 0; i < fireCount; i++)
        {
            animator.SetBool("attack", true);
            laserAttackController.Aim();

            float aimTime = fireAimTime;

            while (aimTime > 0)
            {
                // aim at player smoothly
                float angle = AngPosUtil.GetAngle(transform.position, player.transform.position);
                angle = Mathf.SmoothDampAngle(fireAttack.transform.eulerAngles.z, angle, ref fireCurrentVelocity, fireSmoothTime);
                fireAttack.transform.eulerAngles = new Vector3(0, 0, angle);

                yield return null;
                aimTime -= Time.deltaTime;
            }

            yield return new WaitForSeconds(fireDelay);

            // fire
            laserAttackController.Fire();

            // rotate while firing
            float _fireTime = fireTime;

            while (_fireTime > 0)
            {
                // aim at player smoothly
                float targetAngle = AngPosUtil.GetAngle(transform.position, player.transform.position);
                float angle = fireAttack.transform.eulerAngles.z;
                if (angle < -180) angle += 360;
                if (angle > 180) angle -= 360;
                if (angle > 90 && targetAngle < -90) targetAngle += 360;
                if (angle < -90 && targetAngle > 90) targetAngle -= 360;

                float delta = targetAngle - angle;
                angle += Mathf.Sign(delta) * fireAngleSpeed * Time.deltaTime;

                fireAttack.transform.eulerAngles = new Vector3(0, 0, angle);

                yield return null;
                _fireTime -= Time.deltaTime;
            }
            laserAttackController.Stop();
            animator.SetBool("attack", false);

            // wait for next fire
            yield return new WaitForSeconds(fireTimeBetweenAttack);
        }

        yield return new WaitForSeconds(fireStunTime);

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateOvertimeIE()
    {
        yield return null;
        rb.velocity = Vector2.zero;
        animator.SetBool("specialAttack", true);

        // Prepare
        overtimeParticle.Play();
        yield return new WaitForSeconds(overtimePreDuration);

        // Attack
        overtimeAttack.SetActive(true);
        yield return new WaitForSeconds(overtimeAttackDuration);

        // Stunned
        overtimeAttack.SetActive(false);
        yield return new WaitForSeconds(1.4f);

        animator.SetBool("specialAttack", false);
        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateChargeIE()
    {
        // reset velocity
        rb.velocity = Vector2.zero;

        for (int i = 0; i < chargeCount; i++)
        {
            // Prepare
            // show attack path
            chargeAttackPath.gameObject.SetActive(true);

            float _prepareTime = chargePrepareTime;

            float angle = 0;

            while (_prepareTime > 0)
            {
                // aim at player
                angle = AngPosUtil.GetAngle(transform.position, player.transform.position);

                // rotate attack path
                chargeAttackPath.rotation = Quaternion.Euler(0, 0, angle);

                yield return null;
                _prepareTime -= Time.deltaTime;
            }

            yield return new WaitForSeconds(chargeAttackDelay);

            // Charge
            animator.SetBool("charging", true);
            animator.SetFloat("xDir", player.transform.position.x - transform.position.x);
            rb.velocity = AngPosUtil.GetAngularPos(angle, chargeSpeed);

            yield return new WaitForSeconds(1.5f);

            yield return WaitTilHitWall();
            animator.SetBool("charging", false);
        }

        // Stunned
        chargeAttackPath.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(chargeStunTime);

        animator.SetFloat("xDir", 0);
        StartCoroutine(RandomNextStateIE());
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
        onDeadEvent.Invoke();

        spriteDeathFadeSOC.enabled = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
