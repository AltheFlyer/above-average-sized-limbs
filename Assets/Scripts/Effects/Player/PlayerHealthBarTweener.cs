using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarTweener : MonoBehaviour
{
    public void Activate()
    {
        if (PlayerHealthBarTween.instance != null)
        {
            PlayerHealthBarTween.instance.Tween();

        }
    }
}
