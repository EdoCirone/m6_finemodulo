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

   public void RestartLevel(bool clearSave = false)
{
    Time.timeScale = 1f;

    if (clearSave)
    {
        // elimino il salvataggio per far respawnare i pickup
        SaveManager.DeleteSave();
    }

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

    public void LoadMainMenu(bool? markDead = null)
    {
        Time.timeScale = 1f;

        var data = SaveManager.LoadGame() ?? new SaveData();

        // aggiorna info base
        data.currentLevelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        data.currentLives = GameManager.Instance.CurrentLives;

        // se ti dico esplicitamente cosa fare con isDead, fallo; altrimenti preserva
        if (markDead.HasValue)
            data.isDead = markDead.Value;

        // salva il checkpoint se presente
        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            Vector3 cpPos = CheckpointManager.Instance.GetCurrentCheckpoint().position;
            data.checkpointPos = new float[] { cpPos.x, cpPos.y, cpPos.z };
        }

        SaveManager.SaveGame(data);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
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
            SaveData data = new SaveData
            {
                currentLevelIndex = nextIndex,
                currentLives = GameManager.Instance.CurrentLives, // vite rimaste
                isDead = false
            };

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
            // Prepara il SaveData
            SaveData data = SaveManager.LoadGame() ?? new SaveData();
            data.currentLevelIndex = index;
            data.currentLives = GameManager.Instance.CurrentLives;

            // Salva anche le vite iniziali del livello
            data.livesAtLevelStart = GameManager.Instance.CurrentLives;

            SaveManager.SaveGame(data);

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

}
