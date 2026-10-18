using System.Buffers.Text;
using UnityEngine;
using System.Collections;

public class BombEnemyController : BaseEnemy
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float throwRange = 5f;
    [SerializeField] private float attackCooldown = 3f;

    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform holdPoint;

    [SerializeField] private float holdTime = 0.5f;
    [SerializeField] private float throwDuration = 0.3f;

    private float lastAttackTime;
    private bool isHoldingBomb = false;
    private float holdTimer;
    private GameObject heldBomb;
    new void Update()
    {
        if(isHoldingBomb)
        {
            holdTimer -= Time.deltaTime;
            if(holdTimer <= 0)
                ThrowBomb();
        }
    }
    void FixedUpdate()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return; 
        }

        if (player == null || isHoldingBomb)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > throwRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            if (Time.time - lastAttackTime >= attackCooldown)
                StartHoldingBomb();
        }
    }
    private void StartHoldingBomb()
    {
        isHoldingBomb = true;
        holdTimer = holdTime;
        rb.linearVelocity = Vector2.zero;

        Vector3 spawnPos = holdPoint != null ? holdPoint.position : transform.position;
        heldBomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        Bomb bombScript = heldBomb.GetComponent<Bomb>();
        if (bombScript != null) 
            bombScript.isActive = false;

        heldBomb.transform.SetParent(transform);
    }
    private void ThrowBomb()
    {
        isHoldingBomb = false;
        lastAttackTime = Time.time;

        if (heldBomb != null)
        {
            heldBomb.transform.SetParent(null);
            
            Bomb bombScript = heldBomb.GetComponent<Bomb>();
            if (bombScript != null)
            {
                bombScript.isActive = true;
                bombScript.Activate();
            }

            Vector3 targetPos = player != null ? player.position : heldBomb.transform.position + transform.right;
            StartCoroutine(ThrowBombRoutine(heldBomb, targetPos));
        }
        heldBomb = null;
    }
    private IEnumerator ThrowBombRoutine(GameObject bomb, Vector3 targetPos)
    {
        float elapsed = 0;
        Vector3 startPos = bomb.transform.position;

        while (elapsed < throwDuration && bomb != null)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / throwDuration;
            
            bomb.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        if (bomb != null)
            bomb.transform.position = targetPos;
    }
}
