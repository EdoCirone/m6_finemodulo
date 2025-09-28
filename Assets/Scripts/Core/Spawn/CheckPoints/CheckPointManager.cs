using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Transform _currentCheckpoint;
    private Vector3? pendingCheckpoint = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (pendingCheckpoint.HasValue)
        {
            // crea un "finto" checkpoint alla posizione salvata
            GameObject cp = new GameObject("LoadedCheckpoint");
            cp.transform.position = pendingCheckpoint.Value;
            _currentCheckpoint = cp.transform;
            pendingCheckpoint = null;
        }
    }

    public void SetPendingCheckpoint(Vector3 pos)
    {
        pendingCheckpoint = pos;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        _currentCheckpoint = checkpoint;
        Debug.Log("Checkpoint aggiornato: " + checkpoint.name);
    }

    public Transform GetCurrentCheckpoint()
    {
        return _currentCheckpoint;
    }

    public bool HasCheckpoint() => _currentCheckpoint != null;
}
