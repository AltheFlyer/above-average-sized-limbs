using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class ActionManager : MonoBehaviour
{
    public UnityEvent<Vector2> moveCheck;
    public UnityEvent<Vector2> handDirectionCheck;
    public UnityEvent dash;
    public UnityEvent<Collider2D> pickUpItem;

    public void OnHandAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();

            float horizontalInput = movementInput.x;

            float verticalInput = movementInput.y;

            // Determine the direction the player should move in
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // Invoke the UnityEvent with the movement direction
            handDirectionCheck.Invoke(moveDirection);

        }
        if (context.canceled)
        {
            //Debug.Log("move stopped");
            handDirectionCheck.Invoke(Vector2.zero);
        }
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
