using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] string uniqueID;

    public string GetID() => uniqueID;
    public bool HasID(string id) => uniqueID == id;

    private void Start()
    {
        SaveData data = SaveManager.LoadGame();
        if (data != null && data.collectedIDs != null && System.Array.Exists(data.collectedIDs, id => id == uniqueID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectableManager.Instance.TakeCollectable(this);

        SaveData data = SaveManager.LoadGame() ?? new SaveData();
        var list = new System.Collections.Generic.List<string>();
        if (data.collectedIDs != null) list.AddRange(data.collectedIDs);
        if (!list.Contains(uniqueID)) list.Add(uniqueID);
        data.collectedIDs = list.ToArray();

        SaveManager.SaveGame(data);

        Destroy(gameObject);
    }
}
