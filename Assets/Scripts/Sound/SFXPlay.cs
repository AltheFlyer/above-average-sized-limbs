using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    public string sfxName;

    public void Activate()
    {
        SFXManager.TryPlaySFX(sfxName, gameObject);
    }
}
