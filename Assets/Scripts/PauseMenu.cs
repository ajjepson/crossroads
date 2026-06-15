using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedMenu;

    public GameObject playButton;
    public GameObject mainMenuButton;
    public GameObject resetButton;
    public GameObject quitButton;
    public GameObject backButton;
    public GameObject volumeButton;
    public GameObject pauseText;
    public GameObject volumeText;
    public GameObject volumeSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausedMenu.SetActive(false);
        volumeSlider.SetActive(false);
        volumeText.SetActive(false);
        backButton.SetActive(false);
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
    public void Reset()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
    public void VolumeButton()
    {
        playButton.SetActive(false);
        mainMenuButton.SetActive(false);
        resetButton.SetActive(false);
        quitButton.SetActive(false);
        backButton.SetActive(true);
        volumeText.SetActive(true);
        pauseText.SetActive(false);
        volumeButton.SetActive(false);
        volumeSlider.SetActive(true);
    }
    public void BackButton()
    {
        playButton.SetActive(true);
        mainMenuButton.SetActive(true);
        resetButton.SetActive(true);
        quitButton.SetActive(true);
        backButton.SetActive(false);
        volumeText.SetActive(false);
        pauseText.SetActive(true);
        volumeButton.SetActive(true);
        volumeSlider.SetActive(false);
    }
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit ButtonPressed");
    }
}
