using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHP = 3;
    private int currentHP;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private Transform player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }
    public void takeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
            Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
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
