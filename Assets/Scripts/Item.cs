using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public List<ItemEffect> effects = new ();
    public string itemName;
    public Sprite itemIcon;

    public void AddEffect(Trigger trigger, ItemEffect effectToCopy)
    {
        int index = effects.Count;
        effects.Add(Instantiate(effectToCopy));
        effects[index].SetTrigger(trigger);
        effects[index].Initialize();
    }
}
