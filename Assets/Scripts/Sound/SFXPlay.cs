using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    public string sfxName;

    public void Activate()
    {
        PlayName(sfxName);
    }

    public void PlayName(string name)
    {
        string[] sfxNames = name.Split(',');
        foreach (var sfx in sfxNames)
        {
            sfx.Trim();
        }
        SFXManager.TryPlaySFX(sfxNames, gameObject);
    }
}
