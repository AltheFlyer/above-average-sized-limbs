using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;

    public string description;

    public Sprite sprite;

    public BaseItem item;

    [Header("For items with custom renderers (e.g. animation)")]
    public GameObject additionalRendererOnPickup;
}
