
using UnityEngine;

public class BigHands : BaseItem
{
    public float sizeUpCoefficient = 1.0f;

    public override void PreAttack(AttackData data)
    {
        data.attackSizeMultiplier += sizeUpCoefficient * Mathf.Log(2 * numStacks);
    }
}