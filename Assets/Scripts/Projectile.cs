using System.Data;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;

    public bool isHoming = false;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float homingDelay = 0.1f;
    private float currentDelay;
    public bool isPlayerProjectile = false;

    private Transform target;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDelay = homingDelay;

        Destroy(gameObject, lifetime);

        if (isHoming)
        {
            FindNearestEnemy();
            if (target != null)
            {
                Vector2 oppositeDir = (transform.position - target.position).normalized;
                rb.linearVelocity = oppositeDir * speed;
            }
        }
    }
    void Update()
    {
        if (!isHoming || target == null)
            return;

        if (currentDelay > 0)
            currentDelay -= Time.deltaTime;

        else{}

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        Vector2 currentDir  = rb.linearVelocity.normalized;
        Vector3 newDir = Vector3.RotateTowards(currentDir, directionToTarget, turnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
        rb.linearVelocity = (Vector2)newDir * speed;

        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }
    public void SetDirection(Vector2 direction)
    {
        if(isHoming)
            return;

        if (rb == null) 
            rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerProjectile) 
            {
                PlayerController playerScript = other.GetComponent<PlayerController>();
                if (playerScript != null)
                {
                    playerScript.takeDamage(damage);
                    Debug.Log("Player hit by projectile!");
                }
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (isPlayerProjectile)
            {
                BaseEnemy enemyScript = other.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.takeDamage(damage);
                    Debug.Log("Enemy hit by homing projectile!");
                }
                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }
}
