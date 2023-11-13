using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShaker : MonoBehaviour
{
    public float magnitude = 1f;

    public void Activate()
    {
        if (CameraShake.instance == null)
        {
            GameObject g = Camera.main.gameObject;
            CameraShake c = g.AddComponent<CameraShake>();
        }

        CameraShake.instance.Shake(magnitude);
    }
}
