using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class ActionManager : MonoBehaviour
{
    public UnityEvent<Vector2> moveCheck;
    public UnityEvent<Vector2> attackCheck;
    public UnityEvent dash;
    public UnityEvent<Collider2D> pickUpItem;

    public void OnAttackAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 attackDirectionInput = context.ReadValue<Vector2>();

            float horizontalInput = attackDirectionInput.x;

            float verticalInput = attackDirectionInput.y;

            Vector2 attackDirection = new Vector2(horizontalInput, verticalInput).normalized;

            attackCheck.Invoke(attackDirection);
        }
        if (context.canceled)
        {
            attackCheck.Invoke(Vector2.zero);
        }

        // if (context.control.name == "upArrow")
        // {
        //     attackCheck.Invoke(context.control.name);
        // }

    }
    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();

            float horizontalInput = movementInput.x;

            float verticalInput = movementInput.y;

            // Determine the direction the player should move in
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Invoke the UnityEvent with the movement direction
            moveCheck.Invoke(moveDirection);

        }
        if (context.canceled)
        {
            //Debug.Log("move stopped");
            moveCheck.Invoke(Vector2.zero);
        }
    }
    public void OnDashAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dash.Invoke();
        }
    }

    public void OnPickUpAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LayerMask pickUpMask = LayerMask.GetMask("Item");
            Collider2D Item = Physics2D.OverlapCircle(transform.position, .2f, pickUpMask);
            pickUpItem.Invoke(Item);
        }

    }

}
