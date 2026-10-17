using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{
    public static PlayerItemHolder instance;
    List<Item> items = new ();
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void TriggerItems(Trigger trigger)
    {
        // Highly inefficient and horrible but what can you do
        foreach (Item item in instance.items)
        {
            foreach(ItemEffect effect in item.effects)
            {
                effect.CheckTrigger(trigger);
            }
        }
        
    }
    public static void AddItem(Item itemToAdd)
    {
        instance.items.Add(itemToAdd);
        Debug.Log("Item added");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)){
            TriggerItems(Trigger.OnTest);
        }
    }
}
