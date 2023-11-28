using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime = 5f;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(this.gameObject, destroyTime);
        transform.position += offset;
    }

}
