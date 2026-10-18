using UnityEngine;

public class EnemyController : BaseEnemy
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;
    void FixedUpdate()
    {
        if (afkTimer > 0f) return;
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return; 
        }
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (afkTimer > 0f) return;
        if (collision.gameObject.CompareTag("Player"))
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;

                PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
                if (playerScript != null)
                    playerScript.takeDamage(damage);
            }
    }
}
