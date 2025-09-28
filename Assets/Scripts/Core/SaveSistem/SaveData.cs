using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentLevelIndex;
    public int currentLives;
    public bool isDead;
    public float[] checkpointPos; 
}