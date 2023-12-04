using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShaker : MonoBehaviour
{
    public float magnitude = 1f;

    public void Activate()
    {
        CameraShake.Shake(magnitude);
    }
}
