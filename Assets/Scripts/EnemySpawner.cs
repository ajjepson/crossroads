using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector3 areaSize = new Vector3(10f, 0f, 10f);

    [Header("Wave Settings")]
    [SerializeField] private int totalEnemiesToSpawn = 20;

    private int enemiesSpawned = 0;
=======
    [Header("Interaction")]
    public GameObject interactText;
    private bool playerInRange;

    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector3 areaSize = new Vector3(10f, 0f, 10f);
    public int currentEnemies;

    [Header("Spawner HP System")]
    public int spawnerHP = 2;
    public int maxSpawnerHP = 2;

    private bool canPress = true;
    public float pressCooldown = 1.5f;

    [Header("UI")]
    public FloatingHealthBar healthBar;
>>>>>>> origin/main

    void Start()
    {
        StartCoroutine(SpawnEnemyLoop());
<<<<<<< HEAD
=======

        if (interactText != null)
            interactText.SetActive(false);

        if (healthBar != null)
        {
            healthBar.SetTarget(transform);
            healthBar.UpdateBar(spawnerHP, maxSpawnerHP);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && canPress)
        {
            StartCoroutine(HandlePress());
        }
    }

    IEnumerator HandlePress()
    {
        canPress = false;

        spawnerHP--;

        Debug.Log("Spawner hit! HP left: " + spawnerHP);

        
        if (healthBar != null)
        {
            healthBar.UpdateBar(spawnerHP, maxSpawnerHP);
        }

        if (spawnerHP <= 0)
        {
            DestroySpawner();
            yield break;
        }

        yield return new WaitForSeconds(pressCooldown);
        canPress = true;
    }

    void DestroySpawner()
    {
        StopAllCoroutines();
        Destroy(gameObject);
>>>>>>> origin/main
    }

    IEnumerator SpawnEnemyLoop()
    {
<<<<<<< HEAD
        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Spawner finished spawning enemies.");
        Destroy(gameObject);
=======
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
>>>>>>> origin/main
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = transform.position + new Vector3(
<<<<<<< HEAD
        Random.Range(-areaSize.x / 2, areaSize.x / 2),
        0,
        Random.Range(-areaSize.z / 2, areaSize.z / 2));

        GameObject enemyToSpawn =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(enemyToSpawn, randomPos, Quaternion.identity);
=======
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        currentEnemies++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("archer"))
        {
            playerInRange = true;

            if (interactText != null)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("archer"))
        {
            playerInRange = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
>>>>>>> origin/main
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
