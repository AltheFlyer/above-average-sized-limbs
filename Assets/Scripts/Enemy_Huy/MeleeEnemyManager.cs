using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Follow,
    Attack,
    Die
};

public class MeleeEnemyManager : MonoBehaviour
{
    public PlayerVariables playerVars;
    private GameObject player;
    private EnemyState currentState;
    public float attackRange;
    private bool cooldownAttack = false;
    public int cooldown;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (currentState)
        {
            case (EnemyState.Follow):
                Follow();
                break;

            case (EnemyState.Attack):
                Attack();
                break;

            case (EnemyState.Die):
                break;
        }

        if (Vector2.Distance(this.transform.position, player.transform.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Follow;
        }

    }
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.5f * Time.deltaTime);
    }

    private void Attack()
    {
        if (!cooldownAttack)
        {
            GameManager.instance.DamagePlayer(-1);

            StartCoroutine(Cooldown());
        }

    }

    private IEnumerator Cooldown()
    {
        cooldownAttack = true;
        yield return new WaitForSeconds(cooldown);
        cooldownAttack = false;
    }

    private void SetEnemyDirection()
    {
        if (player.transform.position.x >= this.transform.position.x)
        {
            //enemyAnimator.SetBool("faceRight", true);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            //enemyAnimator.SetBool("faceRight", false);
        }
    }

    // private IEnumerator ChooseDir()
    // {
    //     chooseDir = true;
    //     yield return new WaitForSeconds(2f);
    //     randomDir = new Vector3(0,0, Random.Range(0,360));

    //     Quaternion nextRotation = Quaternion.Euler(randomDir);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
    //     chooseDir = false;
    // }

    // void Wander()
    // {
    //     if (!chooseDir)
    //     {
    //         StartCoroutine(ChooseDir());
    //     }
    //     transform.position -= transform.right*enemySpeed*Time.deltaTime;
    // }


}

