using UnityEngine;

public class BossController : BaseEnemy
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float changeDirectionTime = 1.5f;
    private Vector2 currentDirection;
    private float directionTimer;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int randomBombsCount = 5;
    [SerializeField] private float mapSpawnRadius = 6f; 
    [SerializeField] private float playerSpawnRadius = 2f; 
    [SerializeField] private float bombSpawnCooldown = 1f;
    private float lastBombSpawnTime;

    protected override void Start()
    {
        base.Start();
        PickRandomDirection();
    }

    new void Update()
    {
        base.Update();
        if (afkTimer > 0f) return;

        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0f)
            PickRandomDirection();
    }

    void FixedUpdate()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return; 
        }
        rb.linearVelocity = currentDirection * moveSpeed;
    }

    private void PickRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        currentDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        directionTimer = changeDirectionTime;
    }

    public override void takeDamage(float damage)
    {
        if (Time.time - lastBombSpawnTime >= bombSpawnCooldown)
        {
            lastBombSpawnTime = Time.time;
            SpawnBombs();
        }

        base.takeDamage(damage);
    }
    private void SpawnBombs()
    {
        if (afkTimer > 0f) return;
        if (bombPrefab == null) 
            return;

        for (int i = 0; i < randomBombsCount; i++)
        {
            Vector2 randomPos = (Vector2)transform.position + (Random.insideUnitCircle * mapSpawnRadius);
            Instantiate(bombPrefab, randomPos, Quaternion.identity);
        }

        if (player != null)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector2 playerBombPos = (Vector2)player.position + (randomDir * playerSpawnRadius);
            Instantiate(bombPrefab, playerBombPos, Quaternion.identity);
        }
    }
}