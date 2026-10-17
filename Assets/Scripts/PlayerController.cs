using UnityEngine;
using System.Collections;

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

    [SerializeField] private Sprite attackSprite;
    [SerializeField] private float attackVisDur = 0.1f;

    private float lastAttackTime;
    private float chargeTimer = 0f;
    private bool isCharging = false;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastFacingDirection = Vector2.down;
    public GameObject animAttackSide;
    public GameObject animAttackDown;
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

        GameObject anim;
        if (attackDir.y == 0)
        {
            anim = Instantiate(animAttackSide, transform.position, Quaternion.identity);
            anim.transform.SetParent(transform);
            anim.transform.localScale = new Vector3(attackDir.x, 1, 1);
            anim.transform.localPosition = new Vector3(attackDir.x*1.2f, 0.5f, 0);
            Destroy(anim, 0.74f);
        }
        else
        {
            anim = Instantiate(animAttackDown, transform.position, Quaternion.identity);
            anim.transform.SetParent(transform);
            anim.transform.localScale = new Vector3(1, -attackDir.y, 1);
            anim.transform.localPosition = new Vector3(0, 1.1f * attackDir.y, 0);
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
                EnemyController enemyScript = enemy.GetComponent<EnemyController>();
                if (enemyScript != null)
                    enemyScript.takeDamage(finalDamage);

                RangedEnemyController rangedScript = enemy.GetComponent<RangedEnemyController>();
                if (rangedScript != null)
                    rangedScript.takeDamage(finalDamage);
                
                BossController bossScript = enemy.GetComponent<BossController>();
                if (bossScript != null)
                    bossScript.takeDamage(finalDamage);
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
