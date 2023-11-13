using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameConstants gameConstants;
    public float attackCoolDown;
    private float lastAttackTime;

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


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        activeMoveSpeed = gameConstants.playerSpeed;

        pickUp = gameObject.GetComponent<PickUp>();
        pickUp.Direction = new Vector2(0f, 0f);


    }

    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalAxis, verticalAxis);

        float attackHorizontal = Input.GetAxisRaw("AttackHorizontal");
        float attackVertical = Input.GetAxisRaw("AttackVertical");
        Vector2 attackDirection = new Vector2(attackHorizontal, attackVertical);


        playerAnimator.SetFloat("attackHorizontal", attackHorizontal);
        playerAnimator.SetFloat("attackVertical", attackVertical);
        playerAnimator.SetFloat("moveHorizontal", horizontalAxis);
        playerAnimator.SetFloat("moveVertical", verticalAxis);
        playerAnimator.SetFloat("speed", Mathf.Sqrt(movement.sqrMagnitude));

        AttackCheck(attackDirection);
        DashCheck();

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
        playerAnimator.SetTrigger("attack");

    }

    public void AttackCheck(Vector2 attackDirection)
    {
        if (attackDirection == Vector2.zero)
        { }
        else
        {
            if (Time.time - lastAttackTime >= attackCoolDown)
            {
                Attack();
                lastAttackTime = Time.time;
            }

        }
    }


    void Move(Vector2 movement)
    {
        // check if it doesn't go beyond maxSpeed
        if (playerBody.velocity.magnitude < gameConstants.maxSpeed)
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
            activeMoveSpeed = gameConstants.dashSpeed;
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
                activeMoveSpeed = gameConstants.playerSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

    }

}
