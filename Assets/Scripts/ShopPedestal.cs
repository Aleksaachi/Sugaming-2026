using UnityEngine;

public class ShopPedestal : MonoBehaviour
{
    public int price;
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
        && Input.GetKeyDown(KeyCode.E) 
        && Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= pickupRange
        && PlayerItemHolder.instance.coinCounter >= price)
        {
            taken = true;
            itemDisplay.sprite = null;
            PlayerItemHolder.AddItem(item);
            PlayerItemHolder.instance.AddCoins(-price);
        }
    }
}
