public interface IItem
{
    /// <summary> 
    /// Invoked when the item is first gained. 
    /// (You can probably use this to modify player stats)
    /// </summary>
    void OnPickUp(PlayerManager player);

    /// <summary> 
    /// Invoked when the player initiates an attack
    /// </summary>
    void PreAttack(AttackData data);

    /// <summary> 
    /// Invoked when any player attack hits an enemy.
    /// </summary>
    void OnHit(HitData data);

    /// <summary>
    /// Invoked when a new item of the same type is picked up.
    /// </summary>
    void Stack();
}
