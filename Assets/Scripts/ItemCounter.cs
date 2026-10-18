using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCounter : MonoBehaviour
{
    public static ItemCounter instance;
    public TextMeshProUGUI textBox;
    public Image icon;
    public Sprite defualtIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    public void UpdateItems(Item item, int amount)
    {
        icon.sprite = item.itemIcon;
        textBox.text = "x0" + amount;
    }
    public void UpdateItems()
    {
        icon.sprite = defualtIcon;
        textBox.text = "x00";
    }
}
