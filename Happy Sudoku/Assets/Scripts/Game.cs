using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Sudoku")]
    public GameObject gameCanvas;
    public GameObject sudokuFieldPanel;
    public GameObject fieldPrefab;
    public TMP_Text timeText;
    private Dictionary<Tuple<int, int>, FieldPrefabObject> fieldPrefabObjectDictionary = new();
    private FieldPrefabObject currentHoveredObject;
    private SudokuObject currentSudokuObject;
    private SudokuObject finalObject;
    private float playTime;

    [Header("Control")]
    public GameObject controlPanel;
    public GameObject controlPrefab;

    [Header("Command")]
    public Button informationButton;
    private bool informationButtonActive;

    private void Start()
    {
        CreateSudokuField();
        CreateControlField();
        CreateSudoku();
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        timeText.text = playTime.ToString("00:00");
    }

    private void CreateSudokuField()
    {
        for(int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                GameObject instance = Instantiate(fieldPrefab, sudokuFieldPanel.transform);

                //instance.GetComponentInChildren<TMP_Text>().text =
                // hold position information
                FieldPrefabObject fieldPrefabObject = new(instance, row, column);
                fieldPrefabObjectDictionary.Add(new(row, column), fieldPrefabObject);
                instance.GetComponent<Button>().onClick.AddListener(() => ClickOnFieldPrefab(fieldPrefabObject));
            }
        }
    }
    private void CreateControlField()
    {
        for (int i = 1; i < 10; i++)
        {
            GameObject instance = Instantiate(controlPrefab, controlPanel.transform);
            instance.GetComponentInChildren<TMP_Text>().text = i.ToString();
            ControlPrefabObject controlPrefabObject = new(i);
            instance.GetComponent<Button>().onClick.AddListener(() => ClickOnControlPrefab(controlPrefabObject));
        }
    }
    private void CreateSudoku()
    {
        SudokuGenerator.CreateSudokuObject(out SudokuObject finalObject, out SudokuObject currentSudokuObject);

        this.finalObject = finalObject;
        this.currentSudokuObject = currentSudokuObject;

        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var currentValue = currentSudokuObject.Values[row, column];
                if (currentValue != 0)
                {
                    var fieldObject = fieldPrefabObjectDictionary[new Tuple<int, int>(row, column)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.isChangeable = false;
                }
            }
        }
    }

    private void ClickOnFieldPrefab(FieldPrefabObject fieldPrefabObject)
    {
        Debug.Log($"Clicked on row: {fieldPrefabObject.Row} and column: {fieldPrefabObject.Column}");
        if (fieldPrefabObject.isChangeable)
        {
            // check if null and unselect
            currentHoveredObject?.UnsetHoverMode();

            currentHoveredObject = fieldPrefabObject;
            fieldPrefabObject.SetHoverMode();
        }
    }

    private void ClickOnControlPrefab(ControlPrefabObject controlPrefabObject)
    {
        Debug.Log($"Number: {controlPrefabObject.Number}");
        if (currentHoveredObject != null)
        {
            if (informationButtonActive) 
                currentHoveredObject.SetInfoNumber(controlPrefabObject.Number);
            else
            {
                currentHoveredObject.SetNumber(controlPrefabObject.Number);
            }
        }
    }

    public void ClickOnInformationButton()
    {
        Debug.Log("Click on information button");
        if (informationButtonActive)
        {
            informationButtonActive = false;
            informationButton.GetComponent<Image>().color = new Color(1f, 1f ,1f);
        }
        else
        {
            informationButtonActive = true;
            informationButton.GetComponent<Image>().color = new Color(0.2039216f, 0.5960785f, 0.8588235f);
        }
    }

    public void ClickOnTimeButton()
    {
        timeText.GetComponent<Animation>().Play();
    }

    public void FinishGame()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var fieldObject = fieldPrefabObjectDictionary[new Tuple<int, int>(row, column)];
                if (fieldObject.isChangeable)
                {
                    // richtigen wert im spiel gesetzt
                    if (finalObject.Values[row, column] == fieldObject.CurrentNumber)
                        fieldObject.SetColorGreen();
                    else 
                        fieldObject.SetColorRed();
                }
            }
        } 
    }
}
 