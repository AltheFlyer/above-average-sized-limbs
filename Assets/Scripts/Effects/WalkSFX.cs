using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSFX : MonoBehaviour
{
    public float sfxPeriod = 0.65f;

    Vector2 lastPos;
    float timeTilNextSound = 0;

    public SFXPlay sfxPlay;

    void Update()
    {
        if (timeTilNextSound > 0)
            timeTilNextSound -= Time.deltaTime;
        else
        {
            if (Vector2.Distance(transform.position, lastPos) > 0.01f)
            {
                sfxPlay.Activate(Camera.main.gameObject);
                timeTilNextSound = sfxPeriod;
            }
        }

        lastPos = transform.position;
    }

    void OnValidate()
    {
        if (sfxPlay == null)
            sfxPlay = GetComponent<SFXPlay>();
    }
}
