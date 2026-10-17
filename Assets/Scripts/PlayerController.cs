using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int maxHP = 10;
    private int currentHP;

    [SerializeField] private int baseDamage = 1;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 0.5f;

    [SerializeField] private float maxChargeMultiplier = 3f;
    [SerializeField] private float chargeSpeed = 2f;

    private float lastAttackTime;
    private float chargeTimer = 0f;
    private bool isCharging = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastFacingDirection = Vector2.down;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(moveInput != Vector2.zero)
            lastFacingDirection = moveInput.normalized;

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
        {
            isCharging = true;
            chargeTimer = 0f;
        }
        if (Input.GetMouseButtonDown(0) && isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0f, maxChargeMultiplier / chargeSpeed);
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;
            lastAttackTime = Time.time;
            Attack();
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
    private void Attack()
    {
        float damageMultiplier = 1f + (chargeTimer * chargeSpeed);
        damageMultiplier = Mathf.Clamp(damageMultiplier, 1f, maxChargeMultiplier);
        int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);

        Vector2 attackDir = lastFacingDirection;

        Vector2 hitCenter = (Vector2)transform.position + (attackDir * (attackRange / 2f));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitCenter, attackRange / 2f);

        foreach (Collider2D enemy in hitEnemies)
            if (enemy.CompareTag("Enemy"))
            {
                EnemyController enemyScript = enemy.GetComponent<EnemyController>();
                if (enemyScript != null)
                    enemyScript.takeDamage(finalDamage);

                RangedEnemyController rangedScript = enemy.GetComponent<RangedEnemyController>();
                if (rangedScript != null)
                    rangedScript.takeDamage(finalDamage);
            }
        PlayerItemHolder.TriggerItems(Trigger.OnAttack);
    }

    public void takeDamage(int damage)
    {
        PlayerItemHolder.TriggerItems(Trigger.OnTakeDamage);
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
        
    }
    public void Die()
    {
        ;
    }

}
