using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] int _healtAmmount = 1;
    [SerializeField] string uniqueID; // assegna un ID unico nell’Inspector

    private void Start()
    {
        // Se l’oggetto è già stato raccolto in un salvataggio precedente lo distruggo subito
        SaveData data = SaveManager.LoadGame();
        if (data != null && data.collectedIDs != null)
        {
            foreach (var id in data.collectedIDs)
            {
                if (id == uniqueID)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LifeController lifeController = other.GetComponent<LifeController>();
        if (lifeController != null)
        {
            lifeController.AddHp(_healtAmmount);

            // Aggiorna il salvataggio: aggiungo questo ID
            SaveData data = SaveManager.LoadGame() ?? new SaveData();
            var list = new System.Collections.Generic.List<string>();
            if (data.collectedIDs != null) list.AddRange(data.collectedIDs);
            if (!list.Contains(uniqueID)) list.Add(uniqueID);
            data.collectedIDs = list.ToArray();

            SaveManager.SaveGame(data);

            Destroy(gameObject);
        }
    }
}
