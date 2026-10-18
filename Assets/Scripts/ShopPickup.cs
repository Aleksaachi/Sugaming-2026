using UnityEngine;

public class ShopPickup : MonoBehaviour
{
    public int price;
    public float pickupRange;
    public int health;
    public int coins;
    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerItemHolder.instance.transform.position) <= pickupRange
        && PlayerItemHolder.instance.coinCounter >= price
        && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1)))
        {
            PlayerItemHolder.TriggerItems(Trigger.OnPickup, transform.position);
            PlayerItemHolder.instance.AddCoins(coins);
            PlayerController.instance.currentHP += health;
            PlayerController.instance.currentHP = Mathf.Clamp(PlayerController.instance.currentHP, 0, PlayerController.instance.maxHP);
            PlayerController.instance.UpdateHealthUI();
            PlayerItemHolder.instance.AddCoins(-price);
            Destroy(gameObject);
        }
    }
}
