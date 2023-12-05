using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBag : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        animator.SetTrigger("punched");
    }

}
