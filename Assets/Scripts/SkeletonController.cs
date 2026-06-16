using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("Setup")]
    [SerializeField] private Animator animator;

    [Header("Target Tags")]
    [SerializeField] private string[] targetTags = { "Player", "archer" };

    [Header("Movement")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;

    private EnemyState currentState;

    private UnityEngine.AI.NavMeshAgent agent;

    private Vector3 patrolCenter;
    private Vector3 patrolTarget;

    private Transform currentTarget;
    private HealthScript currentTargetHealth;

    private float lastAttackTime;

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent!");
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(gameObject.name + " is NOT on a NavMesh!");
            enabled = false;
            return;
        }

        patrolCenter = transform.position;

        SetNewPatrolPoint();

        InvokeRepeating(nameof(FindNearestTarget), 0f, 0.5f);
    }

    private void Update()
    {
        if (agent == null)
            return;

        if (!agent.isOnNavMesh)
            return;

        UpdateState();
    }

    private void UpdateState()
    {
        if (currentTarget == null)
        {
            currentState = EnemyState.Patrol;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance <= attackRange)
            {
                currentState = EnemyState.Attack;
            }
            else if (distance <= sightRange)
            {
                currentState = EnemyState.Chase;
            }
            else
            {
                currentState = EnemyState.Patrol;
            }
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void FindNearestTarget()
    {
        float nearestDistance = Mathf.Infinity;

        currentTarget = null;
        currentTargetHealth = null;

        foreach (string tag in targetTags)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    currentTarget = target.transform;
                    currentTargetHealth = target.GetComponent<HealthScript>();
                }
            }
        }
    }

    private void Patrol()
    {
        agent.isStopped = false;

        if (animator != null)
            animator.SetBool("Walking", true);

        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            SetNewPatrolPoint();
        }

        agent.SetDestination(patrolTarget);
    }

    private void SetNewPatrolPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;

        Vector3 candidate = patrolCenter + new Vector3(randomPoint.x, 0f, randomPoint.y);

        UnityEngine.AI.NavMeshHit hit;

        if (UnityEngine.AI.NavMesh.SamplePosition(
            candidate,
            out hit,
            patrolRadius,
            UnityEngine.AI.NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
        }
        else
        {
            patrolTarget = patrolCenter;
        }
    }

    private void Chase()
    {
        if (currentTarget == null)
            return;

        agent.isStopped = false;

        if (animator != null)
            animator.SetBool("Walking", true);

        agent.SetDestination(currentTarget.position);
    }

    private void Attack()
    {
        if (currentTarget == null)
            return;

        agent.isStopped = true;

        if (animator != null)
            animator.SetBool("Walking", false);

        Vector3 lookTarget = currentTarget.position;
        lookTarget.y = transform.position.y;

        transform.LookAt(lookTarget);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (animator != null)
                animator.SetTrigger("Attack");

            if (currentTargetHealth != null)
            {
                currentTargetHealth.TakeDamage(damage);
            }

            lastAttackTime = Time.time;
        }
    }

    public void SetPatrolArea(Vector3 center, float radius)
    {
        patrolCenter = center;
        patrolRadius = radius;
        SetNewPatrolPoint();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
    }


}
    



    
