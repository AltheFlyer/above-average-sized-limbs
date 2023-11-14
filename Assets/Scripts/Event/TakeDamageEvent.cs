using UnityEngine;

[CreateAssetMenu(menuName = "Event/TakeDamage")]
public class TakeDamageEvent : GameEvent<DamageData>
{

}

public class DamageData
{
    public Damageable target;
    public int damage;
    public int postDamageHealth;

    public DamageData(Damageable target, int damage, int postDamageHealth)
    {
        this.target = target;
        this.damage = damage;
        this.postDamageHealth = postDamageHealth;
    }
}