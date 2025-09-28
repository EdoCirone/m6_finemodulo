using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] UI_CoinPanel _coinPanel;
    [SerializeField] UI_LifePanel _lifePanel;
    [SerializeField] UI_Timer _timerPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        // Sub alla sorgente unica delle vite
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged += UpdateLives;
            UpdateLives(GameManager.Instance.CurrentLives); // sync iniziale
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLivesChanged -= UpdateLives;
    }

    public void ResetUI()
    {
        _coinPanel?.UpdateCoinGraphics(0, 0);
        UpdateLives(0);
        _timerPanel?.UpdateTimerUI(0);
    }

    public void RebindUI()
    {
        if (_coinPanel == null) _coinPanel = FindObjectOfType<UI_CoinPanel>();
        if (_lifePanel == null) _lifePanel = FindObjectOfType<UI_LifePanel>();
        if (_timerPanel == null) _timerPanel = FindObjectOfType<UI_Timer>();

        // Sync immediato vite dopo il restart livello
        if (GameManager.Instance != null)
            UpdateLives(GameManager.Instance.CurrentLives);
    }

    public void UpdateCoins(int current, int max) => _coinPanel?.UpdateCoinGraphics(current, max);

    public void UpdateLives(int lives) => _lifePanel?.UpdateLifeDisplay(lives);

    public void UpdateTimer(float timeLeft) => _timerPanel?.UpdateTimerUI(timeLeft);
}
