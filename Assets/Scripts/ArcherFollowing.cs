using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ArcherFollowing : MonoBehaviour
{

    //new
    enum EnemyAIActions {following, attacking }

    private EnemyAIActions currentState;
    private NavMeshAgent agent;

    private GameObject player;
    private float distanacePlayer;

    private const float distanaceAttack = 1f;
    private const float distanaceFollowing = 1000f;
    //new
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //new
        //swordSwing.canPlayerSwing = false;

        currentState = EnemyAIActions.following;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        //new
    }

    // Update is called once per frame
    void Update()
    {

        //new
        if (player == null)
        {
            return;
        }
        DetermineCurrentState();
        FollowingCurrentState();
        //new
    }
    //new
    private void DetermineCurrentState()
    {
        distanacePlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanacePlayer < distanaceAttack)
        {
            currentState = EnemyAIActions.attacking;
        }
        else if(distanacePlayer < distanaceFollowing)
        {
            currentState = EnemyAIActions.following;
        }
        else
        {
            //?
        }
    }
    private void FollowingCurrentState()
    {
        switch (currentState)
        {
            case EnemyAIActions.following:
                agent.SetDestination(player.transform.position);
                break;
            case EnemyAIActions.attacking:
                agent.SetDestination(player.transform.position);
                break;
        }
    }
}
