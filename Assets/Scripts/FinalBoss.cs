using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    public Slider finalhealthBar;
    public TMP_Text finalHealthText;
    private int finalHealth = 400;
    public int finalMaxHealth = 0;
    public SwordSwing swordSwing;

    //new
    enum EnemyAIActions { flying, chasing, attacking }

    private EnemyAIActions currentState;
    private NavMeshAgent agent;

    [SerializeField] private Transform[] checkpoints;
    private int currentCheckPointIndex;

    private GameObject player;
    private float distanacePlayer;

    private const float distanaceAttack = 5f;
    private const float distanaceFollowing = 20f;
    //new
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finalMaxHealth = finalHealth;
        //new
        //swordSwing.canPlayerSwing = false;

        currentState = EnemyAIActions.flying;
        agent = GetComponent<NavMeshAgent>();
        currentCheckPointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player");

        //new
    }

    // Update is called once per frame
    void Update()
    {
        finalHealthText.text = finalHealth + " / " + finalMaxHealth;
        finalhealthBar.value = (float)finalHealth / (float)finalMaxHealth;

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
            currentState = EnemyAIActions.flying;
        }
    }
    private void FollowingCurrentState()
    {
        switch (currentState)
        {
            case EnemyAIActions.flying:

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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("boss took slice damage");
            finalHealth -= 10;
        }
        if (other.CompareTag("Arrows"))
        {
            Debug.Log("boss took arrow damage");
            finalHealth -= 5;
        }
        if (finalHealth <= 0)
        {
            //SceneManager.LoadScene("end");
        }
    }
}
