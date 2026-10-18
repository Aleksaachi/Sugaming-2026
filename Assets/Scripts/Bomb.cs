using UnityEngine;
using UnityEngine.WSA;

public class Bomb : MonoBehaviour
{
    public float destroyAfter;
    public GameObject explosion;
    float timer;
    float bounceTimer;
    public bool isActive = true;

    [SerializeField] private int damage = 1;
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float knockbackForce = 1.5f;
    public SquashAndStretch squashAndStretch;
    void Start()
    {
        if(isActive)
            Activate();
    }
    public void Activate()
    {
        isActive = true;
        squashAndStretch.Play();
        timer = destroyAfter;
        float extraTime = Random.Range(-0.5f, 0.5f);
        timer += extraTime;
        bounceTimer += extraTime;
    }
    void Update()
    {
        if(!isActive)
            return;
            
        bounceTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0)
            Explode();
        if (bounceTimer <= 0)
        {
            squashAndStretch.Play();
            bounceTimer = 0.5f;
        }
    }
    private void Explode()
    {
        GameObject expl = Instantiate(explosion, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Destroy(expl, 0.74f);

        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D obj in objectsInRange)
        {
            Vector2 knockbackDir = (obj.transform.position - transform.position).normalized;

            if (obj.CompareTag("Player"))
            {
                PlayerController player = obj.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.takeDamage(damage);
                    player.ApplyKnockback(knockbackDir, knockbackForce * 10f);
                }
            }
            else if (obj.CompareTag("Enemy"))
            {
                BaseEnemy enemy = obj.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.takeDamage(damage);
                    enemy.ApplyKnockback(knockbackDir, knockbackForce * 10f);
                }
            }
        }

        Destroy(gameObject);
    }
}
