using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{

    public void Start()
    {
        Destroy(GameObject.FindObjectOfType<PlayerManager>());
        Destroy(GameObject.FindObjectOfType<PlayerInventory>());
    }
}