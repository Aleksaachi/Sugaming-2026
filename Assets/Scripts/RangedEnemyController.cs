using UnityEngine;

public class RangedEnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private int maxHP = 2;
    private int currentHP;

    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        if (distanceToPlayer > attackRange)
            rb.linearVelocity = direction * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Ranged Enemy: No Projectile Prefab assigned!");
            return;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 directionToPlayer = (player.position - spawnPos).normalized;
        Projectile projScript = bullet.GetComponent<Projectile>();

        if (projScript != null)
            projScript.SetDirection(directionToPlayer);
    }

    public void takeDamage(int damage)
    {
        PlayerItemHolder.TriggerItems(Trigger.OnDealDamage);
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        Room.currentRoom.enemies.Remove(gameObject);
        PlayerItemHolder.TriggerItems(Trigger.OnKill);
        Destroy(gameObject);
    }
}