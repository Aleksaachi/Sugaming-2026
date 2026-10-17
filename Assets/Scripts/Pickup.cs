using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupRange;
    public int health;
    public int coins;
    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= pickupRange)
        {
            PlayerItemHolder.TriggerItems(Trigger.OnPickup);
            PlayerItemHolder.instance.AddCoins(coins);
            PlayerController.instance.currentHP += health;
            PlayerController.instance.currentHP = Mathf.Clamp(PlayerController.instance.currentHP, 0, PlayerController.instance.maxHP);
            PlayerController.instance.UpdateHealthUI();
            Destroy(gameObject);
        }
    }
}
