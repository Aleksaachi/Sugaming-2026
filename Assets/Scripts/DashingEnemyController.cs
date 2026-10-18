using UnityEngine;

public class DashingEnemyController : BaseEnemy
{
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float chargeDuration = 0.8f;
    [SerializeField] private float dashCooldown = 2f;

    [SerializeField] private int damage = 3;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    private enum State { Walking, Charging, Dashing }
    private State currentState;

    private float stateTimer;
    private float cooldownTimer;
    private Vector2 dashDirection;

    protected override void Start()
    {
        base.Start();
        currentState = State.Walking;
        cooldownTimer = dashCooldown;
    }
    new void Update()
    {
        if (player == null)
            return;

        switch(currentState)
        {
            case State.Walking:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    currentState = State.Charging;
                    stateTimer = chargeDuration;
                }
                break;

            case State.Charging:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    currentState = State.Dashing;
                    stateTimer = dashDuration;
                    dashDirection = (player.position - transform.position).normalized;
                    
                    if (squashAndStretch != null)
                        squashAndStretch.Play();
                }
                break;

            case State.Dashing:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    currentState = State.Walking;
                    cooldownTimer = dashCooldown;
                }
                break;
        }
    }
    void FixedUpdate()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        if (player == null)
            return;

        switch(currentState)
        {
            case State.Walking:
                Vector2 directionToPlayer = (player.position - transform.position).normalized;
                rb.linearVelocity = directionToPlayer * movementSpeed;
                break;

            case State.Charging:
                rb.linearVelocity = Vector2.zero;
            break;

            case State.Dashing:
                rb.linearVelocity = dashDirection * dashSpeed;
                break;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(currentState != State.Dashing)
            return;

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
