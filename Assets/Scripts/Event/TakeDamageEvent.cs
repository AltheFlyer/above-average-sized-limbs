using UnityEngine;

[CreateAssetMenu(menuName = "Event/TakeDamage")]
public class TakeDamageEvent : GameEvent<DamageData>
{

}

public class DamageData
{
    public Damageable target;
    public float damage;
    public float postDamageHealth;

    public DamageData(Damageable target, float damage, float postDamageHealth)
    {
        this.target = target;
        this.damage = damage;
        this.postDamageHealth = postDamageHealth;
    }
}