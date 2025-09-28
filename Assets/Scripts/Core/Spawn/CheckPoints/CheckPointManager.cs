using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    // Buffer statico valido tra scene
    private static Vector3? sPendingCheckpoint = null;
    private static int sPendingLevel = -1;

    private Transform _currentCheckpoint;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Se c'è un checkpoint buffered per questo livello, applicalo
        if (sPendingCheckpoint.HasValue && SceneManager.GetActiveScene().buildIndex == sPendingLevel)
        {
            CreateLoadedCheckpoint(sPendingCheckpoint.Value);
            sPendingCheckpoint = null;
            sPendingLevel = -1;
        }
    }

    private void CreateLoadedCheckpoint(Vector3 pos)
    {
        GameObject cp = new GameObject("LoadedCheckpoint");
        cp.transform.position = pos;
        _currentCheckpoint = cp.transform;

        // assegna al player appena è in scena
        StartCoroutine(AssignToPlayerNextFrame());
    }

    private IEnumerator AssignToPlayerNextFrame()
    {
        // aspetta un paio di frame per essere sicuro che il PlayerRespawn abbia eseguito Start()
        yield return null;
        yield return null;

        var player = FindObjectOfType<PlayerRespawn>();
        if (player != null)
        {
            player.SetSpawnPoint(_currentCheckpoint);
            // Teletrasporta ORA il player al checkpoint (non aspettare la morte)
            player.RespawnHere(_currentCheckpoint);
        }
    }


    public static void SetPendingForNextLoad(Vector3 pos, int levelIndex)
    {
        sPendingCheckpoint = pos;
        sPendingLevel = levelIndex;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        _currentCheckpoint = checkpoint;
        Debug.Log("Checkpoint aggiornato: " + checkpoint.name);
    }

    public Transform GetCurrentCheckpoint() => _currentCheckpoint;
    public bool HasCheckpoint() => _currentCheckpoint != null;
}
