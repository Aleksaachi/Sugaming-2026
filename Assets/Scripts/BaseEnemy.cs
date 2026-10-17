using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected int maxHP = 3;
    protected int currentHP;
    protected float knockbackTimer = 0f;

    protected Rigidbody2D rb;
    protected Transform player;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public virtual void takeDamage(int damage)
    {
        PlayerItemHolder.TriggerItems(Trigger.OnDealDamage);
        currentHP -= damage;
        
        if (currentHP <= 0)
            Die();
    }
    public virtual void ApplyKnockback(Vector2 direction, float force, float duration = 0.15f)
    {
        rb.linearVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }
    protected virtual void Die()
    {
        if (Room.currentRoom != null && Room.currentRoom.enemies != null)
            Room.currentRoom.enemies.Remove(gameObject);
            
        PlayerItemHolder.TriggerItems(Trigger.OnKill);
        Destroy(gameObject);
    }
}