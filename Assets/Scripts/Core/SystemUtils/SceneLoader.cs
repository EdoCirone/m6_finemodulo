using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        StartCoroutine(ResetSceneCoroutine());
    }

    private IEnumerator ResetSceneCoroutine()
    {
        Scene current = SceneManager.GetActiveScene();
        yield return SceneManager.LoadSceneAsync(current.buildIndex);

        yield return null; // aspetta un frame

        UIManager.Instance?.RebindUI();
        UIManager.Instance?.ResetUI();

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            FindObjectOfType<CameraManager>()?.SetPlayer(playerGO.transform);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCreditsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Salvataggio progressione
            SaveData data = new SaveData();
            data.currentLevelIndex = nextIndex;
            data.currentHp = GameManager.Instance.CurrentLives; // vite globali
            data.unlockedLevels = UnlockLevelArray(nextIndex);

            SaveManager.SaveGame(data);

            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            LoadMainMenu();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Ho chiuso il gioco.");
        Application.Quit();
    }

    public void LoadLevelByIndex(int index)
    {
        Time.timeScale = 1f;

        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogWarning($"Indice scena {index} non valido!");
        }
    }

    public void NewGame()
    {
        SaveManager.DeleteSave();
        GameManager.Instance.ResetLivesForNewGame();
        LoadLevelByIndex(1); // livello 1 giocabile
    }

    public void ContinueGame()
    {
        SaveData data = SaveManager.LoadGame();
        if (data != null)
        {
            GameManager.Instance.ResetLivesForNewGame();
            LoadLevelByIndex(data.currentLevelIndex);
        }
        else
        {
            Debug.Log("Nessun salvataggio trovato, avvio nuova partita.");
            NewGame();
        }
    }

    private bool[] UnlockLevelArray(int lastUnlocked)
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings;
        bool[] unlocked = new bool[totalLevels];

        for (int i = 0; i <= lastUnlocked; i++)
            unlocked[i] = true;

        return unlocked;
    }
}
