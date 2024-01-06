using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySetting : MonoBehaviour
{
    public Slider DifficultySlider;
    public TMP_Text DifficultyText;

    private void Start()
    {
        int savedValue = PlayerPrefs.GetInt("difficulty");
        if (savedValue == 0) savedValue = 20;

        DifficultySlider.value = savedValue;
        DifficultyText.text = $"{DifficultySlider.value} von 81 Feldern sichtbar";
        PlayerPrefs.SetInt("difficulty", (int)DifficultySlider.value);
    }

    public void SaveDifficultySetting()
    {
        int value = (int)DifficultySlider.value;
        DifficultyText.text = $"{DifficultySlider.value} von 81 Feldern entfernen";
        Debug.Log(value);
        PlayerPrefs.SetInt("difficulty", value);
    }
}
