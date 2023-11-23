using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarTweener : MonoBehaviour
{
    public void Activate()
    {
        if (PlayerHealthBarTween.instance != null)
        {
            Debug.Log("test");
            PlayerHealthBarTween.instance.Tween();

        }
    }
}
