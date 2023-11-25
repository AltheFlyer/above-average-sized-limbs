using UnityEngine;

/// <summary>
///  Abstract base class for items in game.
/// </summary>
public abstract class BaseItem : MonoBehaviour, IItem
{
    [SerializeField]
    protected int numStacks = 1;

    public virtual void OnPickUp(PlayerManager player) { }
    public virtual void PreAttack(AttackData data) { }
    public virtual void OnHit(HitData data) { }

    public virtual void Stack()
    {
        ++numStacks;
    }
}
