using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    [SerializeField] private GameObject _finishPoint;
    [SerializeField] private UnityEvent _onCollectedall;
    [SerializeField] private UnityEvent _onCollected;

    public List<Collectable> _collectablesInScene { get; private set; }
    public List<Collectable> _playerCollectables { get; private set; } = new List<Collectable>();
    public int _totalCollectables { get; private set; }

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
        _collectablesInScene = new List<Collectable>(FindObjectsOfType<Collectable>());
        _totalCollectables = _collectablesInScene.Count;

        SaveData data = SaveManager.LoadGame();
        if (data != null && data.collectedIDs != null)
        {
            // Rimuovi quelli già raccolti
            foreach (string id in data.collectedIDs)
            {
                Collectable c = _collectablesInScene.Find(x => x.HasID(id));
                if (c != null)
                {
                    _collectablesInScene.Remove(c);
                    _playerCollectables.Add(c);
                }
            }
        }

        UIManager.Instance.UpdateCoins(CollectedCount, _totalCollectables);

        // Se erano già stati presi tutti, attiva subito il finish point
        if (_playerCollectables.Count == _totalCollectables)
            _finishPoint.SetActive(true);
    }

    public void TakeCollectable(Collectable collectable)
    {
        if (_collectablesInScene.Contains(collectable))
        {
            _collectablesInScene.Remove(collectable);
            _playerCollectables.Add(collectable);
            CheckAllCollected();

            _onCollected?.Invoke();
            UIManager.Instance.UpdateCoins(CollectedCount, _totalCollectables);
        }
    }

    private void CheckAllCollected()
    {
        if (_playerCollectables.Count == _totalCollectables)
        {
            Debug.Log("Ho collezionato tutto quanto, spero di invocare il menu");
            _onCollectedall?.Invoke();
            _finishPoint.SetActive(true);
        }
    }

    public int CollectedCount => _playerCollectables.Count;
}
