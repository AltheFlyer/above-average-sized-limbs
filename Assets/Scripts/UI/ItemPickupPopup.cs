using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Component that manages the item pickup prompt. 
/// Should use a GameEventListener for the item pickup event. 
/// Upon pickup, the item enters a queue. If there are no items currently being displayed, 
/// the popup will show the item at the head of the queue (and pop it off), for the specified times.
/// The item queue allows for handling of multiple consecutive item pickups. (Kind of like in Risk of Rain 2)
/// </summary>
public class ItemPickupPopup : MonoBehaviour
{
    // Time needed for the popup to fully fade in
    public float fadeInTime;
    // Time to hold the pickup indicator at max opacity
    public float holdTime;
    // Time needed for the popup to fully fade away
    public float fadeOutTime;
    // The maximum opacity of the item pickup prompt
    public float maxOpacity;

    public GameObject displayParent;
    public Image imageField;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI descField;

    private Queue<ItemData> itemPickupQueue = new Queue<ItemData>();
    private bool isShowingItem = false;

    // Internal variable indiacating how much longer to show the item pickup popup
    private float timeLeft;

    public void OnItemPickup(ItemData item)
    {
        if (!isShowingItem && itemPickupQueue.Count == 0)
        {
            ShowItemPopup(item);
            return;
        }
        itemPickupQueue.Enqueue(item);
    }

    private void ShowItemPopup(ItemData item)
    {
        // Set fields for display
        imageField.sprite = item.sprite;
        nameField.text = item.itemName;
        descField.text = item.description;

        // Stuff to make display parent visible
        displayParent.SetActive(true);

        // Set flag and timer
        isShowingItem = true;
        timeLeft = fadeInTime + holdTime + fadeOutTime;
    }

    public void Update()
    {
        if (timeLeft <= 0)
        {
            if (itemPickupQueue.Count > 0)
            {
                ShowItemPopup(itemPickupQueue.Dequeue());
            }
            else
            {
                isShowingItem = false;
                displayParent.SetActive(false);
            }
        }
        else
        {
            timeLeft -= Time.deltaTime;

            float alpha = 0;

            // Determine transparency based on how long the popup has appeared for
            if (timeLeft > holdTime + fadeOutTime)
            {
                alpha = maxOpacity - (maxOpacity * (timeLeft - holdTime - fadeOutTime) / fadeInTime);
            }
            else if (timeLeft > fadeOutTime)
            {
                alpha = maxOpacity;
            }
            else
            {
                alpha = maxOpacity * (timeLeft) / fadeOutTime;
            }

            SetImageAlpha(displayParent.GetComponent<Image>(), alpha);
            SetImageAlpha(imageField, alpha);
            nameField.alpha = alpha;
            descField.alpha = alpha;
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color oldColor = image.color;
        image.color = new Color(
             oldColor.r, oldColor.g, oldColor.b, alpha
         );
    }
}