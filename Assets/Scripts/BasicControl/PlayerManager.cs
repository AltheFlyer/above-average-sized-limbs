using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameConstants gameConstants;
    private Animator playerAnimator;
    private Rigidbody2D playerBody;
    private bool moving = false;
    private bool alive = true;

    
    private float activeMoveSpeed;

    //For Dashing
    private float dashLength = .5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    //Pick Up item
    private PickUp pickUp;


    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        activeMoveSpeed = gameConstants.playerSpeed;

        pickUp = gameObject.GetComponent<PickUp>();
        pickUp.Direction = new Vector2(0f,0f);


    }

    void Update()
    {    
        // playerAnimator.SetFloat("horizontal", horizontalAxis);
        // playerAnimator.SetFloat("vertical", verticalAxis);
        // playerAnimator.SetFloat("xSpeed", Mathf.Sqrt(movement.sqrMagnitude));

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


    void Move(Vector2 movement)
    {
        // check if it doesn't go beyond maxSpeed
        if (playerBody.velocity.magnitude < gameConstants.maxSpeed)
        {
            playerBody.MovePosition(playerBody.position + movement* activeMoveSpeed * Time.deltaTime);
            //playerBody.velocity = movement * activeMoveSpeed;
        }
            //playerBody.AddForce(movement * speed);
            //playerBody.velocity = movement * activeMoveSpeed;
    }



    public void MoveCheck(Vector2 movement)
    {
        if (movement == Vector2.zero)
        {
            moving = false;
        }
        else
        {
            //FlipMarioSprite(movement.x);
            moving = true;
            Move(movement);
        }
    }

    public void Dash()
    {
        if (dashCounter <= 0 && dashCoolCounter <=0)
        {
            activeMoveSpeed = gameConstants.dashSpeed;
            dashCounter = dashLength;
        }

    }

    public void DashCheck()
    {
        if (dashCounter >0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = gameConstants.playerSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter >0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

    }

}
