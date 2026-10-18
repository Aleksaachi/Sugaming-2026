using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Search;

public class PlayerItemHolder : MonoBehaviour
{
    public int coinCounter = 0;
    public static PlayerItemHolder instance;
    public List<Item> items = new ();
    public Vector3 triggerLocation;
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void TriggerItems(Trigger trigger, Vector3 newTriggerLocation)
    {
        instance.triggerLocation = newTriggerLocation;
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
        ItemCounter.instance.UpdateItems(itemToAdd, instance.items.Count);
    }
    public static void RemoveLastItem()
    {
        instance.items.RemoveAt(instance.items.Count-1);

        if (instance.items.Count != 0) ItemCounter.instance.UpdateItems(instance.items[instance.items.Count-1], instance.items.Count);
        else ItemCounter.instance.UpdateItems();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            PlayerController.instance.Die();
        }
    }

    public void AddCoins(int amount)
    {
        coinCounter += amount;
        CoinCounter.instance.UpdateCoins(coinCounter);
    }
}
