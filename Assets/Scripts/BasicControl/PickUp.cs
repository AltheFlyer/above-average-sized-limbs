using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdSpot;
    //public GameObject destroyEffect;
    public Vector3 Direction { get; set; }
    private GameObject itemHolding = null;
    private Collider2D pickUpItem;
    private bool isHoldingItem = false;
    private bool canThrow = false;

    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Item"))
        {
            pickUpItem = col.gameObject.GetComponent<Collider2D>();
        }
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Item"))
        {
            pickUpItem = null;
        }
    }


    public void PickUpCheck()
    {
        if (pickUpItem && isHoldingItem == false)
        {
            PickUpAction();
            isHoldingItem = true;
            canThrow = false;
            StartCoroutine(WaitAndEnableThrow());
        }
    }

    IEnumerator WaitAndEnableThrow()
    {
        yield return new WaitForSeconds(0.2f);
        canThrow = true;
    }

    public void ThrowCheck()
    {
        if (itemHolding && isHoldingItem && canThrow)
        {
            StartCoroutine(ThrowItem(itemHolding));
            itemHolding = null;
            isHoldingItem = false;
        }

    }

    public void PickUpAction()
    {
        itemHolding = pickUpItem.gameObject;
        itemHolding.transform.position = holdSpot.position;
        itemHolding.transform.parent = transform;
        if (itemHolding.GetComponent<Rigidbody2D>())
            itemHolding.GetComponent<Rigidbody2D>().simulated = false;
    }



    IEnumerator ThrowItem(GameObject item)
    {
        Vector3 startPoint = item.transform.position;
        Vector3 endPoint = transform.position + Direction * 2;
        item.transform.parent = null;
        for (int i = 0; i < 25; i++)
        {
            item.transform.position = Vector3.Lerp(startPoint, endPoint, i * .04f);
            yield return null;
        }
        if (item.GetComponent<Rigidbody2D>())
            item.GetComponent<Rigidbody2D>().simulated = true;
        //Instantiate(destroyEffect, item.transform.position, Quaternion.identity);
        //Destroy(item);
    }

}