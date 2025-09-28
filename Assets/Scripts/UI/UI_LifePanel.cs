using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LifePanel : MonoBehaviour
{
    [SerializeField] private Image lifeIcon;       // opzionale
    [SerializeField] private TMP_Text lifeCounter; // "x 7"

    public void UpdateLifeDisplay(int currentHp)
    {
        if (lifeCounter != null) lifeCounter.text = $"x {currentHp}";
    }
}
