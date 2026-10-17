using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected int maxHP = 3;
    protected float currentHP;
    protected float knockbackTimer = 0f;
    [SerializeField] protected GameObject hitParticlesPrefab;

    protected Rigidbody2D rb;
    protected Transform player;
    public SquashAndStretch squashAndStretch;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public virtual void takeDamage(float damage)
    {
        squashAndStretch.Play();
        PlayerItemHolder.TriggerItems(Trigger.OnDealDamage, transform.position);
        currentHP -= damage;

        if (hitParticlesPrefab != null)
        {
            GameObject particles = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 1.5f);
        }

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
            
        PlayerItemHolder.TriggerItems(Trigger.OnKill, transform.position);
        Destroy(gameObject);
    }
}