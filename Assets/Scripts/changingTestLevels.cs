using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changingTestLevels : MonoBehaviour
{
    private string loadCertainScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       loadCertainScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (loadCertainScene)
            {
                case "IsaiahTest":
                    SceneManager.LoadScene("IsaiahTest2");
                    break;
                case "IsaiahTest2":
                    SceneManager.LoadScene("1-1 Village");
                    break;
                case "1-1 Village":
                    SceneManager.LoadScene("1-2 Meadows");
                    break;
                case "1-2 Meadows":
                    SceneManager.LoadScene("1-3 Cave");
                    break;
                case "1-3 Cave":
                    SceneManager.LoadScene("2-1");
                    break;
                case "2-1":
                    SceneManager.LoadScene("2-2");
                    break;
                case "2-2":
                    SceneManager.LoadScene("2-3");
                    break;
                case "2-3":
                    SceneManager.LoadScene("end");
                    break;
            }
        }
        else if (other.CompareTag("archer"))
        {
            switch (loadCertainScene)
            {
                case "IsaiahTest":
                    SceneManager.LoadScene("IsaiahTest2");
                    break;
                case "IsaiahTest2":
                    SceneManager.LoadScene("1-1 Village");
                    break;
                case "1-1 Village":
                    SceneManager.LoadScene("1-2 Meadows");
                    break;
                case "1-2 Meadows":
                    SceneManager.LoadScene("1-3 Cave");
                    break;
                case "1-3 Cave":
                    SceneManager.LoadScene("2-1");
                    break;
                case "2-1":
                    SceneManager.LoadScene("2-2");
                    break;
                case "2-2":
                    SceneManager.LoadScene("2-3");
                    break;
                case "2-3":
                    SceneManager.LoadScene("end");
                    break;
            }
        }
        else
        {
            //none player touch sceneloader
        }
    }
}
