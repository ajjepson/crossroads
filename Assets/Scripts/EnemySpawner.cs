using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [Header("Spawn Settings")]
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private Vector3 areaSize = new Vector3(10f, 0f, 10f);
    private int currentEnemies;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemyLoop());
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyLoop()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    void SpawnEnemy()
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        currentEnemies++;


    }

    private void OnDrawGizmos()
    {
        
        // Random spawn area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
