using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class Item : MonoBehaviour
{
    [SerializeField] private string uniqueID;

    public string UniqueID => uniqueID;

    protected virtual void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }

    protected virtual void Start()
    {
        // Se l'oggetto è già stato raccolto in un salvataggio precedente → distruggilo
        SaveData data = SaveManager.LoadGame();
        if (data != null && data.collectedIDs != null &&
            Array.Exists(data.collectedIDs, id => id == uniqueID))
        {
            Destroy(gameObject);
        }
    }

    protected void SaveAsCollected()
    {
        SaveData data = SaveManager.LoadGame() ?? new SaveData();
        var list = new System.Collections.Generic.List<string>();
        if (data.collectedIDs != null) list.AddRange(data.collectedIDs);

        if (!list.Contains(uniqueID)) list.Add(uniqueID);
        data.collectedIDs = list.ToArray();

        SaveManager.SaveGame(data);
    }

    protected abstract void OnPickup(Collider other);

    private void OnTriggerEnter(Collider other)
    {
        OnPickup(other);
        SaveAsCollected();
        Destroy(gameObject);
    }
}
