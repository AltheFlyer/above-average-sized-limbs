
using UnityEngine;

public class BigHands : BaseItem
{
    public float sizeMultiplier = 2.0f;

    public override void PreAttack(AttackData data)
    {
        data.attackSizeMultiplier *= sizeMultiplier;
    }
}