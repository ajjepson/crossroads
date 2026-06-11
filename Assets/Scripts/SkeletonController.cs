using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    [Header("Setup")]
    private Vector3 patrolCenter;
    public float patrolRadius = 2f;
    private Vector3 patrolTarget;
    [SerializeField] private Transform player;
    [SerializeField] private FloatingHealthBar healthBar;

    [Header("Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    int health;
    float lastAttackTime;
    HealthScript playerHealth;

    void Start()
    {
        health = maxHealth;
        patrolCenter = transform.position;
        healthBar?.UpdateBar(health, maxHealth);

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                playerHealth = p.GetComponent<HealthScript>();
            }
        }
        else
        {
            playerHealth = player.GetComponent<HealthScript>();
        }

        if (playerHealth == null)
            Debug.LogError("No HealthScript");

        SetNewPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange) currentState = EnemyState.Attack;
        else if (dist <= sightRange) currentState = EnemyState.Chase;
        else currentState = EnemyState.Patrol;

        switch (currentState)
        {
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Chase: Chase(); break;
            case EnemyState.Attack: Attack(); break;
        }
    }

    void Patrol()
    {
        Move(patrolTarget);
        if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            SetNewPatrolPoint();
    }

    void SetNewPatrolPoint()
    {
        Vector2 random = Random.insideUnitCircle.normalized * Random.Range(1f, patrolRadius);
        patrolTarget = new Vector3(
            patrolCenter.x + random.x,
            transform.position.y,
            patrolCenter.z + random.y);
    }

    void Chase()
    {
        Move(player.position);
    }

    void Attack()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange * 0.8f)
            Move(player.position);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage((float)damage);
            else
                Debug.LogWarning("playerHealth is null, can't deal damage!");

            lastAttackTime = Time.time;
        }
    }

    void Move(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(target.x, transform.position.y, target.z),
            speed * Time.deltaTime
        );
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        healthBar?.UpdateBar(health, maxHealth);
        if (health <= 0) Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
    }

    public void SetPatrolArea(Vector3 center, float radius)
    {
        patrolCenter = center;
        patrolRadius = radius;
        SetNewPatrolPoint();
    }


}



    
