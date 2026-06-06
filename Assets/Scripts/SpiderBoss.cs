using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpiderBoss : MonoBehaviour
{
    public Slider spiderhealthBar;
    public TMP_Text spiderHealthText;
    private int spiderHealth = 500;
    public int spiderMaxHealth = 0;
    public SwordSwing swordSwing;

    //new
    enum EnemyAIActions { walking, chasing, attacking }

    private EnemyAIActions currentState;
    private NavMeshAgent agent;

    [SerializeField] private Transform[] checkpoints;
    private int currentCheckPointIndex;

    private GameObject player;
    private float distanacePlayer;

    private const float distanaceAttack = 2f;
    private const float distanaceFollowing = 5.0f;
    //new
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spiderMaxHealth = spiderHealth;
        //new
        //swordSwing.canPlayerSwing = false;

        currentState = EnemyAIActions.walking;
        agent = GetComponent<NavMeshAgent>();
        currentCheckPointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player");

        //new
    }

    // Update is called once per frame
    void Update()
    {
        spiderHealthText.text = spiderHealth + " / " + spiderMaxHealth;
        spiderhealthBar.value = (float)spiderHealth / (float)spiderMaxHealth;

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
        else if (distanacePlayer < distanaceFollowing)
        {
            currentState = EnemyAIActions.chasing;
        }
        else
        {
            currentState = EnemyAIActions.walking;
        }
    }
    private void FollowingCurrentState()
    {
        switch (currentState)
        {
            case EnemyAIActions.walking:

                if (!agent.pathPending && agent.remainingDistance < 1.0f)
                {
                    //close to player
                    agent.SetDestination(checkpoints[currentCheckPointIndex].position);
                    currentCheckPointIndex++;
                    if (currentCheckPointIndex >= checkpoints.Length)
                    {
                        currentCheckPointIndex = 0;
                    }
                }
                break;
            case EnemyAIActions.chasing:
                agent.SetDestination(player.transform.position);
                break;
            case EnemyAIActions.attacking:
                agent.SetDestination(player.transform.position);
                break;
        }
    }
    //new
    public void OnTriggerEnter(Collider other)
    {
        if (swordSwing.canPlayerSwing == true)
        {
                Debug.Log("spider took slice damage");
                spiderHealth -= 10;
        }
        if (other.CompareTag("Arrows"))
        {
            Debug.Log("spider took arrow damage");
            spiderHealth -= 5;
        }
        if ( spiderHealth < 0)
        {
            //loads next level
        }
    }
}
