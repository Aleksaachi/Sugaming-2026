using UnityEngine;

public class ItemPedestal : MonoBehaviour
{
    public SpriteRenderer itemDisplay;
    public float pickupRange;
    Item item;
    bool taken = false;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private float height = 0.2f;
    private Vector3 startPos;
    private float randomOffset;
    void Start()
    {
        // Get a new random item from ItemFactory
        item = ItemFactory.GetItem();
        itemDisplay.sprite = item.itemIcon;

        startPos = itemDisplay.transform.localPosition;
        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }
    void Update()
    {
        if(!taken)
        {
            float newY = startPos.y + Mathf.Sin((Time.time * speed) + randomOffset) * height;
            itemDisplay.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
        if (!taken 
        && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))
        && Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= pickupRange)
        {
            ItemBar.instance.UpdateName(item.itemName);
            taken = true;
            itemDisplay.sprite = null;
            PlayerItemHolder.AddItem(item);
        }
    }
}
