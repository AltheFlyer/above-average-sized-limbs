using UnityEngine;

[CreateAssetMenu(menuName = "Event/PlayerDeath")]
public class PlayerDeathEvent : GameEvent<PlayerDeathData>
{

}

/// <summary>
///
/// </summary>
public class PlayerDeathData
{
    /// <summary>
    /// Warning! The player field should not be stored by any user of this event, 
    /// as the player will cease to exist after the event has completed!
    /// </summary>
    public GameObject player;
    public Vector3 deathLocation;

    public PlayerDeathData(GameObject player)
    {
        deathLocation = player.transform.position;
    }
}
