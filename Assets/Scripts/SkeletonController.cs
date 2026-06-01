using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    [Header("Setup")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
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
    Transform targetPoint;

    void Start()
    {
        health = maxHealth;

        targetPoint = pointA; 

        healthBar?.UpdateBar(health, maxHealth);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange) currentState = EnemyState.Attack;
        else if (dist <= sightRange) currentState = EnemyState.Chase;
        else currentState = EnemyState.Patrol;

        if (currentState == EnemyState.Patrol) Patrol();
        if (currentState == EnemyState.Chase) Chase();
        if (currentState == EnemyState.Attack) Attack();

        if (Input.GetKeyDown(KeyCode.K)) TakeDamage(50);
    }

    void Patrol()
    {
        Move(targetPoint.position);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.8f)
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
    }

    void Chase()
    {
        Move(player.position);
    }

    void Attack()
    {
        Move(transform.position);

        if (Time.time > lastAttackTime + attackCooldown)
        {
            Health p = player.GetComponent<Health>();
            if (p != null) p.TakeDamage(damage);

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



    
