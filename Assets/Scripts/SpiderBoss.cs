using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpiderBoss : MonoBehaviour
{
    public Slider spiderhealthBar;
    public TMP_Text spiderHealthText;
    public int spiderHealth = 200;
    public int spiderMaxHealth = 0;
    public SwordSwing swordSwing;

    public GameObject poisionBreath;

    //for poisionBreathChance not repeating per frame
    private bool breathAttack = false;

    //for runing away
    private bool didSpiderRan = false;
    private float cooldownSpider = 0;
    private float retreatTime = 3;
    private float retreatLength = 10;
    //

    //for webSpit
    public GameObject webSpit;
    public Transform webSpitSpawner;

    //new
    enum EnemyAIActions { walking, chasing, attacking, /*new*/ runaway }

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
        poisionBreath.SetActive(false);
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
        if (spiderHealth <= 0)
        {
            SceneManager.LoadScene("2-1 Frozen Lake");
        }

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

    //handle spider retreat
    public void SpiderTookDamage(int amount)
    {
        spiderHealth -= amount;
        //spider chance of running away when hp is 150 or more is 25%
        if (spiderHealth >=150 && !didSpiderRan && UnityEngine.Random.value <= .25f)
        {
            SpiderBossRan();
        }
        //spider chance of running away when hp is 150-100 is 50%
        else if (spiderHealth >= 100 && spiderHealth < 150 && !didSpiderRan && UnityEngine.Random.value <= .5f)
        {
            SpiderBossRan();
        }
        //spider chance of running away when hp is 100 or less is 75%
        else if (spiderHealth < 100 && !didSpiderRan && UnityEngine.Random.value <= .75f)
        {
            SpiderBossRan();
        }
        else
        {
            //spider did not run
        }
    }
    private void SpiderBossRan()
    {
        didSpiderRan = true;
        currentState = EnemyAIActions.runaway;
        cooldownSpider = retreatTime;

    }
    private void SpiderGettingAway()
    {
        cooldownSpider -= Time.deltaTime;
        Vector3 direction = (transform.position - player.transform.position).normalized;
        Vector3 targetPath = transform.position + direction * retreatLength;
        agent.SetDestination(targetPath);

        if (cooldownSpider <= 0)
        {
            didSpiderRan = false;
            currentState = EnemyAIActions.walking;
        }
    }
    //handle spider retreat

    //new
    private void DetermineCurrentState()
    {
        distanacePlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanacePlayer < distanaceAttack)
        {
            currentState = EnemyAIActions.attacking;
            poisionBreath.SetActive(false);
            breathAttack = false;
        }
        else if (distanacePlayer < distanaceFollowing)
        {
            currentState = EnemyAIActions.chasing;
            if (!breathAttack)
            {
                breathAttack = true;
                //chance of poision attack is 25% 
                if (UnityEngine.Random.value < .25f)
                {
                    poisionBreath.SetActive(true);
                }
                else
                {
                    //poison chance failed
                    poisionBreath.SetActive(false);

                }
            }
        }
        else
        {
            currentState = EnemyAIActions.walking;
            poisionBreath.SetActive(false);
            breathAttack = false;
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
                case EnemyAIActions.runaway:
                SpiderGettingAway();
                    break;
        }
    }
    //new
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrows"))
        {
            Debug.Log("spider took arrow damage");
            spiderHealth -= 5;
        }
    }
}
