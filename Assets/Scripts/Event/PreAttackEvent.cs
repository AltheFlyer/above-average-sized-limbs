using UnityEngine;

[CreateAssetMenu(menuName = "Event/PreAttack")]
public class PreAttackEvent : GameEvent<AttackData>
{

}

public class AttackData
{
    public GameObject attackPrefab;
    public float damage;
    public float attackSizeMultiplier;

    public AttackData(GameObject prefab, float dmg, float atkSizeMult)
    {
        attackPrefab = prefab;
        damage = dmg;
        attackSizeMultiplier = atkSizeMult;
    }
}