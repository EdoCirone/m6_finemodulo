using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Vite globali (valore iniziale)")]
    [SerializeField] private int startingLives = 3;

    [Header("DEBUG – Vite attuali (solo lettura)")]
    [SerializeField] private int debugCurrentLives; // mostrato solo per debug


    public int CurrentLives { get; private set; }


    public event Action<int> OnLivesChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetLivesForNewGame(); // all'avvio parti con le vite piene
    }

    private void SetLives(int value)
    {
        CurrentLives = Mathf.Max(0, value);
        debugCurrentLives = CurrentLives;
        OnLivesChanged?.Invoke(CurrentLives);
    }

    public void ResetLivesForNewGame() => SetLives(startingLives);
    public void SetLivesFromSave(int value) => SetLives(value);

    public void AddLife(int amount = 1) => SetLives(CurrentLives + Mathf.Max(0, amount));

    // NON riavvio i livelli qui ma nel LifeController
    public void LoseLife(int amount = 1)
    {
        SetLives(CurrentLives - Mathf.Max(0, amount));
       
    }
}