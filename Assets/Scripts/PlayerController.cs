using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public int maxHP = 10;
    public int currentHP;

    [SerializeField] private int baseDamage = 1;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float knockbackDist = 1f;

    [SerializeField] private float maxChargeMultiplier = 3f;
    [SerializeField] private float chargeSpeed = 2f;

    [SerializeField] private Sprite attackSprite;
    [SerializeField] private float attackVisDur = 0.1f;

    private float lastAttackTime;
    private float chargeTimer = 0f;
    private bool isCharging = false;

    private float knockbackTimer = 0f;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;
    private float invincibilityTimer = 0f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject hitParticlesPrefab;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastFacingDirection = Vector2.down;
    public GameObject animAttackSide;
    public GameObject animAttackDown;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) 
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHP = maxHP;
        UpdateHealthUI();

        DontDestroyOnLoad(healthBar);
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if(moveInput != Vector2.zero)
            lastFacingDirection = moveInput.normalized;

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
        {
            isCharging = true;
            chargeTimer = 0f;
        }
        if (Input.GetMouseButton(0) && isCharging)
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
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            spriteRenderer.enabled = Mathf.FloorToInt(invincibilityTimer / blinkInterval) % 2 == 0;
        }
        else 
            spriteRenderer.enabled = true;
    }
    void FixedUpdate()
    {
        if (knockbackTimer > 0)
            knockbackTimer -= Time.fixedDeltaTime;
        else
            rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
    public void ApplyKnockback(Vector2 direction, float force, float duration = 0.15f)
    {
        rb.linearVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }
    private void Attack()
    {
        float damageMultiplier = 1f + (chargeTimer * chargeSpeed);
        damageMultiplier = Mathf.Clamp(damageMultiplier, 1f, maxChargeMultiplier);
        int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);

        Vector2 attackDir = lastFacingDirection;

        GameObject anim;
        if (attackDir.y == 0)
        {
            anim = Instantiate(animAttackSide, transform.position, Quaternion.identity);
            anim.transform.SetParent(transform);
            anim.transform.localScale = new Vector3(Mathf.Sign(attackDir.x), 1);
            anim.transform.localPosition = new Vector3(Mathf.Sign(attackDir.x)*1.2f, 0.5f, 0);
            Destroy(anim, 0.74f);
        }
        else
        {
            anim = Instantiate(animAttackDown, transform.position, Quaternion.identity);
            anim.transform.SetParent(transform);
            anim.transform.localScale = new Vector3(1, -Mathf.Sign(attackDir.y), 1);
            anim.transform.localPosition = new Vector3(0, 1.1f * Mathf.Sign(attackDir.y), 0);
            Destroy(anim, 0.74f);
            if (attackDir.y == 1)
            {
                anim.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
        
        StartCoroutine(HitEnemies(0.5f, finalDamage, attackDir));

        
    }
    private IEnumerator HitEnemies(float waitTime, int finalDamage, Vector2 attackDir)
    {
        yield return new WaitForSeconds(waitTime);

        Vector2 hitCenter = (Vector2)transform.position + (attackDir * (attackRange / 2f));

        ShowAttackVisual(hitCenter, attackDir);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitCenter, attackRange / 2f);

        foreach (Collider2D enemy in hitEnemies)
            if (enemy.CompareTag("Enemy"))
            {
                BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.takeDamage(finalDamage);
                    enemyScript.ApplyKnockback(attackDir, knockbackDist * 10f);
                }
            }
        PlayerItemHolder.TriggerItems(Trigger.OnAttack);
    }

    private void ShowAttackVisual(Vector2 position, Vector2 direction)
    {
        if (attackSprite == null)
            return;

        GameObject visualObj = new GameObject("AttackVisual");
        visualObj.transform.position = position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        visualObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        SpriteRenderer sr = visualObj.AddComponent<SpriteRenderer>();
        sr.sprite = attackSprite;

        sr.sortingOrder = 10;

        visualObj.transform.localScale = new Vector3(attackRange, attackRange, 1f);

        Destroy(visualObj, attackVisDur);
    }
    public void takeDamage(int damage)
    {
        if (invincibilityTimer > 0)
            return;

        PlayerItemHolder.TriggerItems(Trigger.OnTakeDamage);
        currentHP -= damage;
        
        if (hitParticlesPrefab != null)
        {
            GameObject particles = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 1.5f);
        }
        UpdateHealthUI();

        if (currentHP <= 0)
            Die();
        else
            invincibilityTimer = invincibilityDuration;
    }
    public void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = GetHealthPercentage();
    }
    public void Die()
    {
        ;
    }
    public float GetHealthPercentage()
    {
        return (float)currentHP/maxHP;
    }
}
