using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerVariables playerVars;
    private Animator playerAnimator;
    private Rigidbody2D playerBody;
    private bool moving = false;
    private bool alive = true;

    private float activeMoveSpeed;

    //For Dashing
    private float dashLength = .5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    //For Dash AfterImage
    private Vector2 lastImagePos;
    public float distanceBetweenImages;

    //Pick Up item
    private PickUp pickUp;

    private float attackPlaceHolder;
    private float[] currentVal;
    private float[] prevVal;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        activeMoveSpeed = playerVars.playerSpeed;

        pickUp = gameObject.GetComponent<PickUp>();
        pickUp.Direction = new Vector2(0f, 0f);

        currentVal = new float[2];
        prevVal = new float[2];

    }

    void Update()
    {
        //Movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        playerAnimator.SetFloat("moveHorizontal", moveHorizontal);
        playerAnimator.SetFloat("moveVertical", moveVertical);
        playerAnimator.SetFloat("speed", Mathf.Sqrt(movement.sqrMagnitude));


        //Attack
        float attackHorizontal = Input.GetAxisRaw("AttackHorizontal");
        float attackVertical = Input.GetAxisRaw("AttackVertical");
        Vector2 attackDirection = new Vector2(attackHorizontal, attackVertical);

        if (attackHorizontal != 0 || attackVertical != 0)
        {
            currentVal[0] = attackHorizontal;
            currentVal[1] = attackVertical;
            prevVal = currentVal;
        }
        else if (attackHorizontal == 0 && attackVertical == 0)
        {
            currentVal = prevVal;
            prevVal = currentVal;
        }

        playerAnimator.SetFloat("attackHorizontal", currentVal[0]);
        playerAnimator.SetFloat("attackVertical", currentVal[1]);

        AttackCheck(attackDirection);

        //Dash
        DashCheck();
    }

    public void ResetCurrentVal()
    {
        currentVal[0] = 0;
        currentVal[1] = 0;
    }

    void FixedUpdate()
    {
        if (alive && moving)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
            MoveCheck(movement);

            //Throw Action
            if (movement.sqrMagnitude > 0.5f)
            {
                pickUp.Direction = movement;
            }
        }
    }


    void Attack()
    {
        GlobalEventHandle.instance?.preAttack.Raise(new AttackData());
        // if (attackDirection.x > 0)
        // {
        playerAnimator.SetTrigger("attack");
        //}
    }

    public void AttackCheck(Vector2 attackDirection)
    {
        if (attackDirection == Vector2.zero)
        { }
        else
        {
            Attack();
        }
    }


    void Move(Vector2 movement)
    {
        // check if it doesn't go beyond maxSpeed
        if (playerBody.velocity.magnitude < playerVars.maxSpeed)
        {
            playerBody.MovePosition(playerBody.position + movement * activeMoveSpeed * Time.deltaTime);
            //playerBody.velocity = movement * activeMoveSpeed;
            //playerBody.AddForce(movement * speed);
        }
    }



    public void MoveCheck(Vector2 movement)
    {
        if (movement == Vector2.zero)
        {
            moving = false;
        }
        else
        {
            moving = true;
            Move(movement);
        }
    }

    public void Dash()
    {
        if (dashCounter <= 0 && dashCoolCounter <= 0)
        {
            activeMoveSpeed = playerVars.dashSpeed;
            dashCounter = dashLength;

            //Put in update
            if (Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }
        }

    }

    public void DashCheck()
    {


        if (dashCounter > 0)
        {
            if (Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = playerVars.playerSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

    }

    public void RecalculateStats(ItemData item)
    {
        // TODO: Check if not dashing
        activeMoveSpeed = playerVars.playerSpeed;
    }

}
