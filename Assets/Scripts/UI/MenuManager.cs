using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menu Panels")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _gameOverMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

    public void TogglePause()
    {
        if (_pauseMenu.activeSelf)
            HidePauseMenu();
        else
            ShowPauseMenu();
    }

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f;
        _gameOverMenu.SetActive(true);
    }

    public void ShowWinMenu()
    {
        Time.timeScale = 0f;
        _winMenu.SetActive(true);
    }

    // Collegamento Al DOn't destroy
    public void RestartLevel()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.RestartLevel();
    }

    public void NextLevel()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadNextLevel();
    }

    public void BackToMenu()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadMainMenu();
    }


    //MainMenu 
    public void ShowCredits()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadCreditsMenu();
    }

    public void QuitGame()
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.QuitGame();
    }
}
