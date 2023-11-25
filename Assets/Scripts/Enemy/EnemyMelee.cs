using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Damageable
{
    [Header("Enemy Melee")]
    public float speed;
    public float attackRange;
    public float preAttackTime;
    public float postAttackTime;
    public GameObject attackPrefab;

    GameObject player;

    Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        GlobalEventHandle.instance.enemySpawn.Raise(new EnemySpawnData(gameObject));

        StartCoroutine(StateMoveIE());
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(1);
        }
    }

    IEnumerator StateMoveIE()
    {
        if (player == null) yield break;

        Vector2 deltaToPlayer = player.transform.position - transform.position;

        while (deltaToPlayer.sqrMagnitude > attackRange * attackRange)
        {
            deltaToPlayer = player.transform.position - transform.position;

            rb.velocity = deltaToPlayer.normalized * speed;

            yield return null;
        }

        StartCoroutine(StateAttackIE());
    }

    IEnumerator StateAttackIE()
    {
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(preAttackTime);

        Attack();

        yield return new WaitForSeconds(postAttackTime);

        StartCoroutine(StateMoveIE());
    }

    private void Attack()
    {
        //Debug.Log("Enemy attacking");

    }

    public override void OnDead()
    {
        GlobalEventHandle.instance.enemyDeath.Raise(new EnemyDeathData(gameObject));

        base.OnDead();
    }
}
