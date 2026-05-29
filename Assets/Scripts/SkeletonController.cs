using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState currentState;

    //CONFIGURATION 
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform player;
    [SerializeField] public FloatingHealthBar floatingHealthBar;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    //[Header("Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int explsoionChangePercent = 30;
    [SerializeField] private GameObject explosionEffect;


    [SerializeField] private float groundCheckDistance = 2f;

    private int currentHealth;
    private Vector3 patrolTarget;

    void Start()
    {
        TryExplode();
        currentHealth = 30;
        currentHealth = maxHealth;
        patrolTarget = pointB.position;
        currentState = EnemyState.Patrol;

        if (floatingHealthBar != null)
            floatingHealthBar.UpdateBar(currentHealth, maxHealth);
    }

    void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50);
        }
    }

    void UpdateState()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
            currentState = EnemyState.Attack;

        else if (distance <= sightRange)
            currentState = EnemyState.Chase;

        else
            currentState = EnemyState.Patrol;
    }

    // partol
    void HandlePatrol()
    {
        MoveTo(patrolTarget);

        if (Vector3.Distance(transform.position, patrolTarget) < 0.3f)
        {
            patrolTarget = (patrolTarget == pointA.position)
                ? pointB.position
                : pointA.position;
        }

        CheckGround();
    }

    // Chase
    void HandleChase()
    {
        MoveTo(player.position);
        CheckGround();
    }

    //Attack
    void HandleAttack()
    {
        MoveTo(transform.position); // stop moving

        Debug.Log("Skeleton Attacking Player!");

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }

    }

    void MoveTo(Vector3 target)
    {
        Vector3 fixedTarget = new Vector3(
            target.x,
            transform.position.y,
            target.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            fixedTarget,
            speed * Time.deltaTime
        );

        RotateTowards(fixedTarget);
    }

    // Rotate
    void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (direction.x >= 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            transform.rotation = Quaternion.Euler(0, -90, 0);
    }

   
    void CheckGround()
    {
        RaycastHit hit;

        if (!Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, groundCheckDistance))
        {
            // no ground ahead → switch patrol direction
            patrolTarget = (patrolTarget == pointA.position)
                ? pointB.position
                : pointA.position;
        }
    }

    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("HP: " + currentHealth);

        if (floatingHealthBar != null)
            floatingHealthBar.UpdateBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            TryExplode();
        }
        else if (currentHealth <= 30)
        {
            TryExplode();
        }

    }
    void TryExplode()
    {
        int chance = Random.Range(0, 3);
        if (chance < explsoionChangePercent)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Die();
        }
        Debug.Log("Enemy exploaded)");
    }

    void Die()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 3f);
    }

    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pointA) Gizmos.DrawWireSphere(pointA.position, 0.3f);

        Gizmos.color = Color.red;
        if (pointB) Gizmos.DrawWireSphere(pointB.position, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
