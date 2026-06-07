using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 3f;

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
            Vector3 randomOffset = new Vector3(
                Random.Range(-5f, 5f),
                0,
                Random.Range(-5f, 5f)
            );

            Instantiate(enemyPrefab, spawnPoint.position + randomOffset, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void OnDrawGizmos()
    {
        if (spawnPoint == null) return;

        // Random spawn area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPoint.position, 1f);
    }
}
