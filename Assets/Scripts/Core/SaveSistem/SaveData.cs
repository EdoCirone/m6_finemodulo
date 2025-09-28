

[System.Serializable]
public class SaveData
{
    public int currentLevelIndex;
    public int currentLives;
    public int livesAtLevelStart;
    public bool isDead;
    public float[] checkpointPos;

    // Nuovo: elenco oggetti raccolti/distrutti
    public string[] collectedIDs;
}

