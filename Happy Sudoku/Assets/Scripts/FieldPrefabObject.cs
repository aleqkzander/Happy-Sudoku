using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldPrefabObject
{
    private readonly GameObject Instance;
    public int Row;
    public int Column;
    public int CurrentNumber;
    public bool isChangeable = true;

    public FieldPrefabObject(GameObject instance, int row, int column)
    {
        Instance = instance;
        Row = row;
        Column = column;
    }

    public bool TryGetTextByName(string name, out TMP_Text text)
    {
        text = null;
        TMP_Text[] texts = Instance.GetComponentsInChildren<TMP_Text>();
        foreach (var currentText in texts)
        {
            if (currentText.name.Contains(name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }

    public void SetColorGreen()
    {
        Instance.GetComponent<Image>().color = new Color(0.1803922f, 0.8f, 0.4431373f);
    }

    public void SetColorRed() 
    {
        Instance.GetComponent<Image>().color = new Color(0.9058824f, 0.2980392f, 0.2352941f);
    }

    public void SetHoverMode()
    {
        // set blue
        Instance.GetComponent<Image>().color = new Color(0.2039216f, 0.5960785f, 0.8588235f);
    }
    public void UnsetHoverMode()
    {
        // set white
        Instance.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void SetNumber(int number)
    {
        if (TryGetTextByName("Number", out TMP_Text tmpText))
        {
            tmpText.text = number.ToString();
            CurrentNumber = number;

            // delete info numbers
            for (int i = 1; i < 10; i++)
            {
                if (TryGetTextByName(i.ToString(), out TMP_Text note)) 
                    note.text = string.Empty;
            }

            UnsetHoverMode();
        }
    }
    public void SetInfoNumber(int number)
    {
        if (TryGetTextByName(number.ToString(), out TMP_Text tmpText))
        {
            tmpText.text = number.ToString();

            if (TryGetTextByName("Number", out TMP_Text value)) 
                value.text = string.Empty;
        }
    } 
}
