using UnityEngine;

public class ItemPedestal : MonoBehaviour
{
    public SpriteRenderer itemDisplay;
    public float pickupRange;
    Item item;
    bool taken = false;
    
    void Start()
    {
        // Get a new random item from ItemFactory
        item = ItemFactory.GetItem();
        itemDisplay.sprite = item.itemIcon;
    }
    void Update()
    {
        if (!taken 
        &&Input.GetKeyDown(KeyCode.E) 
        && Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= pickupRange)
        {
            taken = true;
            itemDisplay.sprite = null;
            PlayerItemHolder.AddItem(item);
        }
    }
}
