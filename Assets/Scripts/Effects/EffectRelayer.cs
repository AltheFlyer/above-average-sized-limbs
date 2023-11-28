using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectRelayer : MonoBehaviour
{
    public UnityEvent OnActivate;

    [ContextMenu("Activate")]
    public void Activate()
    {
        OnActivate.Invoke();
    }
}
