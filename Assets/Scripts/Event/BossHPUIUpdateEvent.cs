using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/BossHPUI")]
public class BossHPUIUpdateEvent : GameEvent<BossHPUIData>
{

}

public struct BossHPUIData
{
    public BossID bossID;
    public float hpRatio;

    public BossHPUIData(BossID bossID, float hpRatio)
    {
        this.bossID = bossID;
        this.hpRatio = hpRatio;
    }
}

public enum BossID
{
    Manager = 0,
    CEO = 1
}