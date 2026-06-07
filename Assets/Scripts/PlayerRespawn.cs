using UnityEngine;


public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPosition;
    private GameObject spawnObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("respawnPoint");

        if (spawn != null)
        {
            respawnPosition = spawn.transform.position;
            transform.position = respawnPosition;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
  
    public void Respawn()
    {
        transform.position = respawnPosition;
    }
    private void OnDrawGizmos()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("respawnPoint");
        if (spawn != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawn.transform.position, 0.75f);
        }
    }
}

