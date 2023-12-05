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
    private bool gameFinished;
    private float playTime;

    [Header("Control")]
    public GameObject controlPanel;
    public GameObject controlPrefab;
    public TMP_Text matchStateText;

    [Header("Command")]
    public Button informationButton;
    private Color informationButtonStandardColor;
    private bool informationButtonActive;
    public Button backButton;
    public Button finishedButton;

    private void Start()
    {
        informationButtonStandardColor = informationButton.GetComponent<Image>().color;
        CreateSudokuField();
        CreateControlField();
        CreateSudoku();
    }

    private void Update()
    {
        if (!gameFinished)
        {
            playTime += Time.deltaTime;
            timeText.text = playTime.ToString("00:00");
        }
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
        if (gameFinished) return;

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
        if (gameFinished) return;

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
            informationButton.GetComponent<Image>().color = informationButtonStandardColor;
        }
        else
        {
            informationButtonActive = true;
            informationButton.GetComponent<Image>().color = new Color(0.945098f, 0.7686275f, 0.05882353f);
        }
    }

    public void ClickOnTimeButton()
    {
        timeText.GetComponent<Animation>().Play();
    }

    public void FinishGame()
    {
        if (gameFinished) return;
        bool hasWrong = false;

        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                var fieldObject = fieldPrefabObjectDictionary[new Tuple<int, int>(row, column)];
                if (fieldObject.isChangeable)
                {
                    // richtigen wert im spiel gesetzt
                    if (finalObject.Values[row, column] == fieldObject.CurrentNumber)
                    {
                        fieldObject.SetColorGreen();
                    }
                    else
                    {
                        fieldObject.SetColorRed();
                        hasWrong = true;
                    }
                }
            }
        }

        if (hasWrong)
            matchStateText.text = "Maybe next time :(";
        else
            matchStateText.text = "Well solved!";

        finishedButton.enabled = false;
        informationButton.enabled = false;
        backButton.GetComponent<Animation>().Play();
        gameFinished = true;
    }
}
 