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
        if (GameManager.Instance != null)
            GameManager.Instance.ResetLivesForNewGame(); // riparti con vite piene

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

    public void NewGame()
    {
        SaveManager.DeleteSave();                  // elimina vecchio salvataggio
        GameManager.Instance.ResetLivesForNewGame();
        SceneLoader.Instance.LoadLevelByIndex(1);  // livello 1
    }

    public void ContinueGame()
    {
        SaveData data = SaveManager.LoadGame();
        if (data == null)
        {
            NewGame();
            return;
        }

        int levelToLoad = data.currentLevelIndex;

        if (data.isDead)
        {
            // Se il salvataggio era in stato di morte  riparti con vite piene
            GameManager.Instance.ResetLivesForNewGame();
        }
        else
        {
            // Se invece hai completato il livello  riparti con le vite rimaste
            GameManager.Instance.SetLivesFromSave(data.currentLives);
        }

        // Prepara eventuale checkpoint salvato
        if (data.checkpointPos != null && data.checkpointPos.Length == 3)
        {
            Vector3 savedPos = new Vector3(data.checkpointPos[0], data.checkpointPos[1], data.checkpointPos[2]);
            CheckpointManager.Instance.SetPendingCheckpoint(savedPos);
        }

        SceneLoader.Instance.LoadLevelByIndex(levelToLoad);
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
