using UnityEngine;

public class ItemEffect : ScriptableObject
{
    Trigger trigger;
    public void CheckTrigger(Trigger check)
    {
        if (trigger == check) Activate();
    }
    public virtual void Activate()
    {
        // To create new effects create a new script that inherits ItemEffect and overrides Activate
    }
    public virtual void Initialize()
    {
        // If your new effects need to be further randomized or just need a start function override Initialize
    }
    public void SetTrigger(Trigger newTrigger)
    {
        trigger = newTrigger;
    }
}

public enum Trigger
{
    None,
    OnAttack,
    OnTakeDamage,
    OnDealDamage,
    OnKill,
    OnTest
}