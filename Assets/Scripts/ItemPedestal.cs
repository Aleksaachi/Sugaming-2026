using UnityEngine;

public class ItemPedestal : MonoBehaviour
{
    public SpriteRenderer itemDisplay;
    Item item;
    void Start()
    {
        // Get a new random item from ItemFactory
        item = ItemFactory.GetItem();
        itemDisplay.sprite = item.itemIcon;
    }
}
