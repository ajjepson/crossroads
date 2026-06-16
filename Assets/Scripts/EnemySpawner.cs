using UnityEngine;
using System.Collections;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector3 areaSize = new Vector3(10f, 0f, 10f);

    [Header("Wave Settings")]
    [SerializeField] private int totalEnemiesToSpawn = 20;

    private int enemiesSpawned = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemyLoop());
    }

    IEnumerator SpawnEnemyLoop()
    {
        while (enemiesSpawned < totalEnemiesToSpawn)
        {
            SpawnEnemy();
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Spawner finished spawning enemies.");
        Destroy(gameObject);
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = transform.position + new Vector3(
        Random.Range(-areaSize.x / 2, areaSize.x / 2),
        0,
        Random.Range(-areaSize.z / 2, areaSize.z / 2));

        GameObject enemyToSpawn =
            enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(enemyToSpawn, randomPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
