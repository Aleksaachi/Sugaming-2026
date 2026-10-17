using System.Data;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 direction)
    {
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
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.takeDamage(damage);
                Debug.Log("Player hit by projectile!");
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
