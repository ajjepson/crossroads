<<<<<<< HEAD
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
=======
using TMPro;
using UnityEngine;
using UnityEngine.AI;
>>>>>>> origin/main
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
<<<<<<< HEAD
    private float retreatLength = 35;
    //

    //for webSpit
    public GameObject webSpit;
    public Transform webSpitSpawner;
    private float cooldownspit = 4;
    private float cooldownTimer = 0;
    //

    //for audio
    [SerializeField] private AudioClip spiderSprayAudio;
    private AudioSource spiderAudioSource;
    //
=======
    private float retreatLength = 10;

>>>>>>> origin/main
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
<<<<<<< HEAD
        spiderAudioSource = GetComponent<AudioSource>();
=======
>>>>>>> origin/main
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
<<<<<<< HEAD
        //for web spit
        cooldownTimer -= Time.deltaTime;
        //
=======
>>>>>>> origin/main
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

<<<<<<< HEAD
    //for web spit
    private void spitAtPlayer()
    {
        if (cooldownTimer <= 0)
        {
            Instantiate(webSpit, webSpitSpawner.position, webSpitSpawner.rotation);
            cooldownTimer = cooldownspit;
        }
    }
    //for web spit


=======
>>>>>>> origin/main
    //handle spider retreat
    public void SpiderTookDamage(int amount)
    {
        spiderHealth -= amount;
        //spider chance of running away when hp is 150 or more is 25%
        if (spiderHealth >=150 && !didSpiderRan && UnityEngine.Random.value <= .25f)
        {
<<<<<<< HEAD
            Debug.Log("spider run");
=======
>>>>>>> origin/main
            SpiderBossRan();
        }
        //spider chance of running away when hp is 150-100 is 50%
        else if (spiderHealth >= 100 && spiderHealth < 150 && !didSpiderRan && UnityEngine.Random.value <= .5f)
        {
<<<<<<< HEAD
            Debug.Log("spider run");
=======
>>>>>>> origin/main
            SpiderBossRan();
        }
        //spider chance of running away when hp is 100 or less is 75%
        else if (spiderHealth < 100 && !didSpiderRan && UnityEngine.Random.value <= .75f)
        {
<<<<<<< HEAD
            Debug.Log("spider run");
=======
>>>>>>> origin/main
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

<<<<<<< HEAD

    //for spider breathcool down
    private IEnumerator PoisonCoolDown()
    {
        poisionBreath.SetActive(true);
        spiderAudioSource.Play();
        yield return new WaitForSeconds(2.5f);

        poisionBreath.SetActive(false);
        spiderAudioSource.Stop();
        breathAttack = false;
    }
    //new
    private void DetermineCurrentState()
    {
        if (currentState == EnemyAIActions.runaway)
        {
            return;
        }
=======
    //new
    private void DetermineCurrentState()
    {
>>>>>>> origin/main
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
<<<<<<< HEAD
                    //spiderAudioSource.clip = spiderSprayAudio;
                    //spiderAudioSource.Play();
                    //poisionBreath.SetActive(true);
                    StartCoroutine(PoisonCoolDown());
=======
                    poisionBreath.SetActive(true);
>>>>>>> origin/main
                }
                else
                {
                    //poison chance failed
                    poisionBreath.SetActive(false);
<<<<<<< HEAD
                    spiderAudioSource.Stop();
=======
>>>>>>> origin/main

                }
            }
        }
        else
        {
<<<<<<< HEAD
            spiderAudioSource.Stop();
=======
>>>>>>> origin/main
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
<<<<<<< HEAD
                if (UnityEngine.Random.value < .5f)
                {
                    spitAtPlayer();
                }
                else
                {
                    //failed 50%
                }
=======
>>>>>>> origin/main
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
<<<<<<< HEAD
            SpiderTookDamage(5);
=======
            spiderHealth -= 5;
>>>>>>> origin/main
        }
    }
}
