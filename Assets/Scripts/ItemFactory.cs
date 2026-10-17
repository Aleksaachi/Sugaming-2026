using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory instance { get; private set;}
    public List<Sprite> itemSprites;
    public List<string> itemNames;
    public List<Trigger> allTriggers;
    public List<ItemEffect> allEffects;
    void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static Item GetItem()
    {
        // Generate a random item
        Item newItem = (Item)ScriptableObject.CreateInstance("Item");

        newItem.itemName = instance.itemNames[Random.Range(0, instance.itemNames.Count)];
        newItem.itemIcon = instance.itemSprites[Random.Range(0, instance.itemSprites.Count)];
        
        int effectAmount = Random.Range(1, 2);
        for (int i = 0; i < effectAmount; i++)
        {
            Trigger trigger = instance.allTriggers[Random.Range(0, instance.allTriggers.Count)];
            ItemEffect effect = instance.allEffects[Random.Range(0, instance.allEffects.Count)];
            newItem.AddEffect(trigger, effect);
        }

        return newItem;
    }
}
