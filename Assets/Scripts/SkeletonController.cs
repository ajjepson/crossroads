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
    [SerializeField] private FloatingHealthBar healthBar;

    [Header("Target Tags")]
    [SerializeField] private string[] targetTags = { "Player", "archer" };

    [Header("Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    int health;
    float lastAttackTime;

    // Nearest target this frame
    Transform currentTarget;
    HealthScript currentTargetHealth;

    void Start()
    {
        health = maxHealth;
        patrolCenter = transform.position;
        healthBar?.UpdateBar(health, maxHealth);
        SetNewPatrolPoint();
    }

    void Update()
    {
        FindNearestTarget();

        if (currentTarget == null)
        {
            currentState = EnemyState.Patrol;
            Patrol();
            return;
        }

        float dist = Vector3.Distance(transform.position, currentTarget.position);

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

    // Scans all tagged targets and locks onto the closest one
    void FindNearestTarget()
    {
        float nearestDist = Mathf.Infinity;
        Transform nearest = null;
        HealthScript nearestHealth = null;

        foreach (string tag in targetTags)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject t in targets)
            {
                float dist = Vector3.Distance(transform.position, t.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = t.transform;
                    nearestHealth = t.GetComponent<HealthScript>();
                }
            }
        }

        currentTarget = nearest;
        currentTargetHealth = nearestHealth;
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
        Move(currentTarget.position);
    }

    void Attack()
    {
        float dist = Vector3.Distance(transform.position, currentTarget.position);
        if (dist > attackRange * 0.8f)
            Move(currentTarget.position);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            if (currentTargetHealth != null)
                currentTargetHealth.TakeDamage((float)damage);
            else
                Debug.LogWarning("Target has no HealthScript!");

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



    
