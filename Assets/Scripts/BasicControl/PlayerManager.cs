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
    private float dashDuration; //how long the player can dash
    private float dashCooldown;  //cooldown period between dashes.
    private float dashDurationCounter;  // Countdown for dashDuration
    private float dashCoolCounter;  // Countdown for dashCooldown

    //For Dash AfterImage
    private Vector2 lastImagePos;
    public float distanceBetweenImages;

    //Pick Up item
    private PickUp pickUp;

    private float attackPlaceHolder;
    private float[] currentAttackInputVal;
    private float[] prevAttackInputVal;

    private string[] attackSFX = { "punch1", "punch2", "punch3" };

    private float currentAttackCooldown;

    public GameObject attackPrefab;

    public IntVariable comboCount;
    public FloatVariable comboTimer;

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        //movement
        activeMoveSpeed = playerVars.playerSpeed;
        dashDuration = playerVars.dashDuration;
        dashCooldown = playerVars.dashCooldown;

        //For pickup items
        pickUp = gameObject.GetComponent<PickUp>();
        pickUp.Direction = new Vector2(0f, 0f);

        //Helper val to check attack
        currentAttackInputVal = new float[2];
        prevAttackInputVal = new float[2];

        //Set combo to 0 when game starts
        comboCount.SetValue(0);

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

        //If any arrow keys is pressed, set Animator Attack Float to currentAttackInputVal 
        if (attackHorizontal != 0 || attackVertical != 0)
        {
            currentAttackInputVal[0] = attackHorizontal;
            currentAttackInputVal[1] = attackVertical;
            prevAttackInputVal = currentAttackInputVal;
        }
        // If no arrow key is pressed, set Animator Attack Float to prevAttackInputVal
        else if (attackHorizontal == 0 && attackVertical == 0)
        {
            currentAttackInputVal = prevAttackInputVal;
            prevAttackInputVal = currentAttackInputVal;
        }

        playerAnimator.SetFloat("attackHorizontal", currentAttackInputVal[0]);
        playerAnimator.SetFloat("attackVertical", currentAttackInputVal[1]);

        AttackCheck(attackDirection);

        //Dash
        DashCheck();

        // attack cooldown tick
        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        // Combo cooldown

        // Update the combo timer
        if (comboTimer.Value > 0)
        {
            comboTimer.ApplyChange(-Time.deltaTime);

            // Check if the combo timer has reached zero
            if (comboTimer.Value <= 0)
            {
                // Reset the combo if the timer has elapsed
                comboCount.SetValue(0);
            }
        }
    }

    public void ResetCurrentAttackInputVal()
    {
        currentAttackInputVal[0] = 0;
        currentAttackInputVal[1] = 0;
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


    // Precondition: attack cooldown is <= 0, attackDirection is non-zero
    void Attack(Vector2 attackDirection)
    {
        currentAttackCooldown = playerVars.attackCooldown;

        GameObject rawAttackObject = Instantiate(attackPrefab, transform.position, Quaternion.identity, transform);

        AttackData data = new AttackData(
            rawAttackObject, playerVars.attackDamage, 1.0f
        );

        GlobalEventHandle.instance?.preAttack.Raise(data);

        rawAttackObject.GetComponent<PlayerAttack>().InitAttack(data);

        // Offset the attack position so larger attacks have more range
        // Pretty janky, but this should do th trick.
        // (At this point, the attack hasn't updated its collider, so we calculate its new size)
        rawAttackObject.transform.position += new Vector3(attackDirection.x, attackDirection.y, 0) * (rawAttackObject.GetComponent<Collider2D>().bounds.extents.x * data.attackSizeMultiplier);

        // rotate if vertical
        if (attackDirection.y == 1)
        {
            rawAttackObject.transform.Rotate(new Vector3(0, 0, 90), Space.Self);
            // Since we attack from our feet, we need to move this up to compensate
            // Yes, adding to the x copmonent moves it up due to rotation jank
            rawAttackObject.transform.Translate(0.5f, 0, 0);

            //...but we also need to move the sprite back
            rawAttackObject.GetComponentInChildren<SpriteRenderer>().transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
        else if (attackDirection.y == -1)
        {
            rawAttackObject.transform.Rotate(new Vector3(0, 0, -90), Space.Self);

            //...but we also need to move the sprite back
            rawAttackObject.GetComponentInChildren<SpriteRenderer>().transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }

        // if (attackDirection.x > 0)
        // {
        playerAnimator.SetTrigger("attack");
        SFXManager.TryPlaySFX(attackSFX, gameObject);
        //}
    }

    public void AttackCheck(Vector2 attackDirection)
    {
        if (attackDirection == Vector2.zero)
        { }
        else if (currentAttackCooldown <= 0)
        {
            Attack(attackDirection);
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
        if (dashDurationCounter <= 0 && dashCoolCounter <= 0)
        {
            activeMoveSpeed = playerVars.dashSpeed;
            //Set dashDurationCounter to dashDuration (how long they player will dash). The cooldown will be done in DashCheck
            dashDurationCounter = dashDuration;

            //Put in update
            if (Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }

            SFXManager.TryPlaySFX("dash1", gameObject);
        }

    }

    public void DashCheck()
    {
        if (dashDurationCounter > 0)
        {
            if (Vector2.Distance(transform.position, lastImagePos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImagePos = transform.position;
            }
            dashDurationCounter -= Time.deltaTime;

            // Set countdown to dashDurationCounter. When it goes below 0, player will stop dashing
            // Set dashCoolCounter to dashCooldown - cooldown time before player can dash again
            if (dashDurationCounter <= 0)
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
