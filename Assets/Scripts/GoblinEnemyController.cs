using UnityEngine;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GoblinEnemyController : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("Target")]
    public string targetTag = "Player";

    [Header("Movement")]
    public float patrolRadius = 5f;
    public float sightRange = 10f;
    public float attackRange = 2f;

    [Header("Combat")]
    public int damage = 10;
    public float attackCooldown = 1.5f;

    [Header("Jump Attack")]
    public float jumpAttackForce = 6f;
    public float jumpAttackUpForce = 3f;
    public float jumpAttackDuration = 0.4f;

    private State state;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private Transform target;
    private HealthScript targetHealth;

    private Vector3 patrolCenter;
    private Vector3 patrolPoint;

   
    private float nextAttackTime;
    private bool isJumpAttacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        patrolCenter = transform.position;

        PickNewPatrolPoint();
    }

    private void Update()
    {
        if (agent == null || rb == null)
            return;

        if (isJumpAttacking)
            return;

        FindTarget();
        ChooseState();

        if (state == State.Patrol)
            Patrol();
        else if (state == State.Chase)
            Chase();
        else if (state == State.Attack)
            Attack();
    }

    private void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag(targetTag);

        if (player == null)
        {
            target = null;
            targetHealth = null;
            return;
        }

        target = player.transform;
        targetHealth = player.GetComponent<HealthScript>();
    }

    private void ChooseState()
    {
        if (target == null)
        {
            state = State.Patrol;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
            state = State.Attack;
        else if (distance <= sightRange)
            state = State.Chase;
        else
            state = State.Patrol;
    }

    private void Patrol()
    {
        agent.isStopped = false;
        agent.SetDestination(patrolPoint);

        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            PickNewPatrolPoint();
        }
    }

    private void Chase()
    {
        if (target == null)
            return;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        if (target == null)
            return;

        agent.isStopped = true;

        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(JumpAttack());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private IEnumerator JumpAttack()
    {
        isJumpAttacking = true;

        agent.enabled = false;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        rb.linearVelocity = Vector3.zero;

        rb.AddForce(
            direction * jumpAttackForce + Vector3.up * jumpAttackUpForce,
            ForceMode.Impulse
        );

        yield return new WaitForSeconds(jumpAttackDuration);

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= attackRange + 1f && targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.2f);

        agent.enabled = true;
        agent.isStopped = false;

        isJumpAttacking = false;
    }

    private void PickNewPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;

        Vector3 randomPoint = patrolCenter + new Vector3(
            randomCircle.x,
            0,
            randomCircle.y
        );

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
        }
        else
        {
            patrolPoint = patrolCenter;
        }
    }

  
}
