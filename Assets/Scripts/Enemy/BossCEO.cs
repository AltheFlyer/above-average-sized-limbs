using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YUtil;

public class BossCEO : Damageable
{
    [Header("Boss CEO")]
    public Transform exclaimationEffectPos;
    public GameObject exclaimationEffect;
    public GameObject deathEffect;
    public ScaleOpacityCurve spriteDeathFadeSOC;

    public Collider2D attackCollider;

    [Header("Hand Grenade")]
    public GameObject handGrenadePrefab;
    public int handGrenadeCount = 6;
    public float handGrenadeDelay = 0.5f;
    public float handGrenadePreTime = 1.2f;
    public float handGrenadePostTime = 3f;
    public Vector2 handGrenadeSpawnCornerSpawnFromCamera = new Vector2(5, 3);

    [Header("VP")]
    public GameObject vpPrefab;
    public Vector2 vpCornerSpawnFromCamera = new Vector2(5, 3);
    public float vpPreTime = 4f;
    public float vpPostTime = 23f;

    [Header("Duplicate")]
    public GameObject duplicatePrefab;
    public GameObject duplicateAim;
    public float duplicatePreTime = 2.1f;
    public float duplicatePostTime = 8f;
    public Vector2 duplicateSpawnVelocityCorner = new Vector2(4, 3);

    [Header("Teleport")]
    public GameObject teleportAttackPrefab;
    public float teleportPreTime = 0.5f;
    public float teleportAttackDelay = 0.7f;
    public float teleportPosTime = 2f;
    public float teleportOffsetDist = 2f;

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

        if (player == null) yield break;

        // Show exclamation mark
        if (exclaimationEffect != null)
            Instantiate(exclaimationEffect, exclaimationEffectPos.position, Quaternion.identity, transform);

        // wait a bit
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RandomNextStateIE());
    }

    public enum State
    {
        None = -1,
        HandGrenade = 0,
        VP = 1,
        Duplicate = 2,
        Teleport = 3
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

        switch (state)
        {
            case State.HandGrenade:
                StartCoroutine(StateHandGrenadeIE());
                break;
            case State.VP:
                StartCoroutine(StateVPIE());
                break;
            case State.Duplicate:
                StartCoroutine(StateDuplicateIE());
                break;
            case State.Teleport:
                StartCoroutine(StateTeleportIE());
                break;
            default:
                break;
        }

        yield return null;
    }

    IEnumerator StateHandGrenadeIE()
    {
        rb.velocity = Vector2.zero;

        // show pre attack
        yield return new WaitForSeconds(handGrenadePreTime);

        // spawn hand grenade
        for (int i = 0; i < handGrenadeCount; i++)
        {
            Vector2 spawnPos = Camera.main.transform.position + new Vector3(Random.Range(-handGrenadeSpawnCornerSpawnFromCamera.x, handGrenadeSpawnCornerSpawnFromCamera.x), Random.Range(-handGrenadeSpawnCornerSpawnFromCamera.y, handGrenadeSpawnCornerSpawnFromCamera.y), 0);
            Instantiate(handGrenadePrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(handGrenadeDelay);
        }

        // wait post attack
        yield return new WaitForSeconds(handGrenadePostTime);

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateVPIE()
    {
        rb.velocity = Vector2.zero;

        // show pre attack
        yield return new WaitForSeconds(vpPreTime);

        // spawn vps
        Vector2 spawnPos1 = Camera.main.transform.position + new Vector3(vpCornerSpawnFromCamera.x, vpCornerSpawnFromCamera.y, 0);
        Vector2 spawnPos2 = Camera.main.transform.position + new Vector3(vpCornerSpawnFromCamera.x, -vpCornerSpawnFromCamera.y, 0);
        Vector2 spawnPos3 = Camera.main.transform.position + new Vector3(-vpCornerSpawnFromCamera.x, vpCornerSpawnFromCamera.y, 0);
        Vector2 spawnPos4 = Camera.main.transform.position + new Vector3(-vpCornerSpawnFromCamera.x, -vpCornerSpawnFromCamera.y, 0);
        List<GameObject> gs = new List<GameObject>();
        gs.Add(Instantiate(vpPrefab, spawnPos1, Quaternion.identity));
        gs.Add(Instantiate(vpPrefab, spawnPos2, Quaternion.identity));
        gs.Add(Instantiate(vpPrefab, spawnPos3, Quaternion.identity));
        gs.Add(Instantiate(vpPrefab, spawnPos4, Quaternion.identity));

        // wait post attack
        float _time = vpPostTime;
        while (vpPostTime > 0)
        {
            bool allDead = true;
            foreach (GameObject g in gs)
            {
                if (g != null)
                {
                    allDead = false;
                    break;
                }
                else continue;
            }

            // if all dead, wait a bit then break
            if (allDead)
            {
                Debug.Log("test");
                yield return new WaitForSeconds(5);
                break;
            }

            yield return null;
            _time -= Time.deltaTime;
        }

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateDuplicateIE()
    {
        rb.velocity = Vector2.zero;

        // setting
        Vector2 spawnVelo1 = new Vector2(duplicateSpawnVelocityCorner.x, duplicateSpawnVelocityCorner.y);
        Vector2 spawnVelo2 = new Vector2(duplicateSpawnVelocityCorner.x, -duplicateSpawnVelocityCorner.y);
        Vector2 spawnVelo3 = new Vector2(-duplicateSpawnVelocityCorner.x, duplicateSpawnVelocityCorner.y);
        Vector2 spawnVelo4 = new Vector2(-duplicateSpawnVelocityCorner.x, -duplicateSpawnVelocityCorner.y);

        // show pre attack
        Instantiate(duplicateAim, transform.position, Quaternion.Euler(0, 0, AngPosUtil.GetAngle(Vector2.zero, spawnVelo1)));
        Instantiate(duplicateAim, transform.position, Quaternion.Euler(0, 0, AngPosUtil.GetAngle(Vector2.zero, spawnVelo2)));
        Instantiate(duplicateAim, transform.position, Quaternion.Euler(0, 0, AngPosUtil.GetAngle(Vector2.zero, spawnVelo3)));
        Instantiate(duplicateAim, transform.position, Quaternion.Euler(0, 0, AngPosUtil.GetAngle(Vector2.zero, spawnVelo4)));
        yield return new WaitForSeconds(duplicatePreTime);

        // spawn duplicate
        GameObject duplicate = null;
        duplicate = Instantiate(duplicatePrefab, transform.position, Quaternion.identity);
        duplicate.GetComponent<AttackBounce>().SetVelocity(spawnVelo1);
        duplicate = Instantiate(duplicatePrefab, transform.position, Quaternion.identity);
        duplicate.GetComponent<AttackBounce>().SetVelocity(spawnVelo2);
        duplicate = Instantiate(duplicatePrefab, transform.position, Quaternion.identity);
        duplicate.GetComponent<AttackBounce>().SetVelocity(spawnVelo3);
        duplicate = Instantiate(duplicatePrefab, transform.position, Quaternion.identity);
        duplicate.GetComponent<AttackBounce>().SetVelocity(spawnVelo4);

        // wait post attack
        yield return new WaitForSeconds(duplicatePostTime);

        StartCoroutine(RandomNextStateIE());
    }

    IEnumerator StateTeleportIE()
    {
        rb.velocity = Vector2.zero;

        // show pre attack
        yield return new WaitForSeconds(teleportPreTime);

        Vector2 playerPos = player.transform.position;

        // teleport
        int randDir = Random.Range(0, 2) * 2 - 1;
        Vector2 teleportPos = playerPos + new Vector2(teleportOffsetDist * randDir, 0);
        transform.position = teleportPos;

        // wait attack
        yield return new WaitForSeconds(teleportAttackDelay);

        // attack
        GameObject attack = Instantiate(teleportAttackPrefab, player.transform.position, Quaternion.identity);

        // wait post attack
        yield return new WaitForSeconds(teleportPosTime);

        StartCoroutine(RandomNextStateIE());
    }

    public override void OnDead()
    {
        attackCollider.enabled = false;

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(handGrenadeSpawnCornerSpawnFromCamera.x * 2, handGrenadeSpawnCornerSpawnFromCamera.y * 2, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(vpCornerSpawnFromCamera.x, vpCornerSpawnFromCamera.y, 0), 0.3f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(vpCornerSpawnFromCamera.x, -vpCornerSpawnFromCamera.y, 0), 0.3f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-vpCornerSpawnFromCamera.x, vpCornerSpawnFromCamera.y, 0), 0.3f);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-vpCornerSpawnFromCamera.x, -vpCornerSpawnFromCamera.y, 0), 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(duplicateSpawnVelocityCorner.x, duplicateSpawnVelocityCorner.y, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(duplicateSpawnVelocityCorner.x, -duplicateSpawnVelocityCorner.y, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(-duplicateSpawnVelocityCorner.x, duplicateSpawnVelocityCorner.y, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(-duplicateSpawnVelocityCorner.x, -duplicateSpawnVelocityCorner.y, 0));
    }
}
