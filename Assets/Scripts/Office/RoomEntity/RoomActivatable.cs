using UnityEngine;

abstract public class RoomActivatable : MonoBehaviour
{
    public abstract void OnRoomFirstEnter();
    public abstract bool IsRoomCleared();
}