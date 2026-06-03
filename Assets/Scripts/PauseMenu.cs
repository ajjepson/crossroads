using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausedMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausedMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void ContiuneButton()
    {
        Time.timeScale = 1.0f;
        pausedMenu.SetActive(false);
    }
    public void MenuPausedButton()
    {
        SceneManager.LoadScene("MenuScreen");
        Time.timeScale = 1.0f;
    }
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit ButtonPressed");
    }
}
