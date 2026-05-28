using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public enum ArcherState { Idle, Patrol, Attack }
    public ArcherState currentState;

    [Header("References")]
    [SerializeField] private Transform pointA, pointB, player;
    [SerializeField] public FloatingHealthBar floatingHealthBar;
   

    [Header("Stats")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float postAttackIdleTime = 1f;
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private float postAttackTimer;
    private Vector3 patrolTarget;
   
    

    void Start()
    {
        currentHealth = maxHealth;
        patrolTarget = pointB.position;
      
        currentState = ArcherState.Patrol;

        if (floatingHealthBar != null)
        {
            floatingHealthBar.UpdateBar(currentHealth, maxHealth);
        }
    }

    void Update()
    {
     
    }

    void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            currentState = ArcherState.Attack;
        }
    }

    void HandleIdleState()
    {
        

       
    }

    void HandlePatrolState()
    {
        //transform.position = Vector3.MoveTowards(transform.position, patrolTarget, speed * Time.deltaTime);

        // Lerp anim speed
      
       

        Vector3 direction = (patrolTarget - transform.position);
        if (direction.sqrMagnitude > 0.001f)
        {
            RotateTowards(direction.normalized);
        }

        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            // Switch to idle state with random timer
            currentState = ArcherState.Idle;
            
        }
    }

    void HandleAttackState()
    {
       ;
       

        Vector3 dirToPlayer = player.position - transform.position;
        dirToPlayer.y = 0;

        RotateTowardsPlayer();

     
    }
    void RotateTowards(Vector3 direction)
    {
        if (direction.z > 0f)
            transform.rotation = Quaternion.Euler(0, 0, 0); // Facing forward
        else if (direction.z < 0f)
            transform.rotation = Quaternion.Euler(0, 180, 0); // Facing backward
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        if (directionToPlayer.z > 0f)
            transform.rotation = Quaternion.Euler(0, 0, 0); // Facing forward
        else
            transform.rotation = Quaternion.Euler(0, 180, 0); // Facing backward
    }

 

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        StopAllCoroutines();

        if (floatingHealthBar != null)
            floatingHealthBar.UpdateBar(currentHealth, maxHealth);

        

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
       

        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Destroy(gameObject, 5f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pointA != null) Gizmos.DrawWireSphere(pointA.position, 0.3f);

        Gizmos.color = Color.red;
        if (pointB != null) Gizmos.DrawWireSphere(pointB.position, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
