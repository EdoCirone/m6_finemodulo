using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{

    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onFallDeath;
    [SerializeField] private UnityEvent _onHit;

    [Header("Morte per caduta")]
    [SerializeField] private float _deathHeight = -10f;

    [Header("pausa per animazioni")]
    [SerializeField] private float _hitPauseDuration = 0.2f;
    [SerializeField] private float _fallDeathPauseDuration = 0.2f;

    private bool _isDead = false;

    private IRespawnable _respawnable;

    private void Start()
    {
        _respawnable = GetComponent<IRespawnable>();

        // Inizializza UI con le vite correnti globali
        if (GameManager.Instance != null)
            UIManager.Instance.UpdateLives(GameManager.Instance.CurrentLives);
    }

    private void Update()
    {
        if (!_isDead && transform.position.y < _deathHeight)
        {
            DieForFalling();
        }
    }

    public void AddHp(int hp) 
    {
        if (_isDead) return;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddLife(hp);
            UIManager.Instance.UpdateLives(GameManager.Instance.CurrentLives);
        }
    }

    public void RemoveHp(int hp) // diventa "perdi una vita" e respawn se >0
    {
        if (_isDead) return;

        _onHit?.Invoke();
        StartCoroutine(HitPauseAndHandleDamage());
    }

    public void Revive()
    {
        _isDead = false;
        if (GameManager.Instance != null)
            UIManager.Instance.UpdateLives(GameManager.Instance.CurrentLives);
    }

    private IEnumerator HitPauseAndHandleDamage()
    {
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_hitPauseDuration);
        Time.timeScale = originalTimeScale;

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife(1);

        if (GameManager.Instance != null && GameManager.Instance.CurrentLives > 0)
        {
            UIManager.Instance.UpdateLives(GameManager.Instance.CurrentLives);

            if (CheckpointManager.Instance.HasCheckpoint())
                _respawnable?.RespawnHere(CheckpointManager.Instance.GetCurrentCheckpoint());
            else
                _respawnable?.RespawnHere(null);

            Revive();
        }
        else
        {
            MenuManager.Instance.ShowGameOverMenu();
        }
    }


    private void DieForFalling()
    {
        if (_isDead) return;
        _isDead = true;
        _onFallDeath?.Invoke();
        StartCoroutine(FallDeathPauseCoroutine());
    }

    private IEnumerator FallDeathPauseCoroutine()
    {
        yield return new WaitForSeconds(_fallDeathPauseDuration);

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife(1);

        // Dopo la caduta → SEMPRE Game Over Menu
        MenuManager.Instance.ShowGameOverMenu();
    }


}


