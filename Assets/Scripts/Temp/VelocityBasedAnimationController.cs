using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityBasedAnimationController : MonoBehaviour
{
    public Animator animator;

    Vector3 oldPosition;

    void FixedUpdate()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        Vector2 delta = transform.position - oldPosition;
        int val = 0;
        if (delta.magnitude > 0.001f)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                // x
                if (delta.x > 0)
                {
                    // right 1
                    val = 1;
                }
                else
                {
                    // left 2
                    val = 2;
                }
            }
            else
            {
                // y
                if (delta.y > 0)
                {
                    // up 3
                    val = 3;
                }
                else
                {
                    // down 4
                    val = 4;
                }
            }
        }

        animator.SetInteger("Direction", val);

        oldPosition = transform.position;
    }
}
