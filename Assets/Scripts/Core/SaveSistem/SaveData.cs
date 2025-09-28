using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // mi salvo le vite, l'indice del livello, i livelli sbloccati e il checkpoint per il salvataggio in partita

    public int currentLevelIndex;
    public int currentHp;
    public bool[] unlockedLevels;
    public float[] checkpointPos;

}
