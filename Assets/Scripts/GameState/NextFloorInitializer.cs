using UnityEngine;

public class NextFloorInitializer : MonoBehaviour
{

    public void Start()
    {
        GameObject.FindObjectOfType<PlayerManager>().transform.position = new Vector3(0, 0, 0);
    }
}