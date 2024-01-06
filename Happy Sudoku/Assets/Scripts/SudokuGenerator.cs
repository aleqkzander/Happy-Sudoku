using System;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator
{
    private static SudokuObject finalSudokuObject;

    public static void CreateSudokuObject(out SudokuObject finalObject, out SudokuObject sudokuGame)
    {
        finalSudokuObject = null;
        SudokuObject sudoku = new();
        InsertRandomGroups(sudoku);

        if (TryToSolve(sudoku))
        {
            sudoku = finalSudokuObject;
        }
        else
        {
            new Exception("Some went wrong when solving");
        }

        finalObject = sudoku;
        sudokuGame = RemoveNumbersFromSudokuObject(sudoku);
    }

    private static SudokuObject RemoveNumbersFromSudokuObject(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new()
        {
            Values = (int[,])sudokuObject.Values.Clone()
        };

        List<Tuple<int ,int>> values = GetListWithValues();

        //int endOfLoopValue = 10;
        //endOfLoopValue = PlayerPrefs.GetInt("difficulty");
        //if (GameSettings.difficutlyLevel == 1) endOfLoopValue = 71;
        //if (GameSettings.difficutlyLevel == 2) endOfLoopValue = 61;
        int endOfLoopValue = PlayerPrefs.GetInt("difficulty");

        bool isFinish = false;

        while (!isFinish)
        {
            int index = UnityEngine.Random.Range(0, values.Count);
            var searchedIndex = values[index];

            SudokuObject nextSudokuObject = new()
            {
                Values = (int[,])newSudokuObject.Values.Clone()
            };

            nextSudokuObject.Values[searchedIndex.Item1, searchedIndex.Item2] = 0;

            values.RemoveAt(index);
            if (TryToSolve(newSudokuObject, true))
            {
                newSudokuObject = nextSudokuObject;
            }

            if (values.Count < endOfLoopValue)
            {
                isFinish = true;
            }
        }

        return newSudokuObject;
    }
    private static List<Tuple<int, int>> GetListWithValues()
    {
        List<Tuple<int, int>> values = new();

        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                values.Add(new Tuple<int,int>(row, column));
            }
        }
        
        return values;
    }

    private static bool TryToSolve(SudokuObject sudokuObject, bool onlyOne = false)
    {
        // find all empty fields
        if (HasEmptyFieldToInsert(sudokuObject, out int row, out int column, onlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObject, row, column);
            foreach (var possibleValue in possibleValues)
            {
                SudokuObject nextInstance = new() {
                    Values = (int[,])sudokuObject.Values.Clone()
                };

                nextInstance.Values[row, column] = possibleValue;
                if (TryToSolve(nextInstance, onlyOne))
                {
                    return true;
                }
            }
        }

        if (HasEmptyField(sudokuObject))
            return false;

        if (onlyOne)
            return true;

        else
        {
            finalSudokuObject = sudokuObject;
            return true;
        }
    }
    private static bool HasEmptyField(SudokuObject sudokuObject)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                // find a field that is empty
                if (sudokuObject.Values[i, j] == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
    private static bool HasEmptyFieldToInsert(SudokuObject sudokuObject, out int row, out int column, bool onlyOne = false)
    {
        row = 0; column = 0;
        int smallestAmountOfPossibleValues = 10;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                // find a field that is empty
                if (sudokuObject.Values[i, j] == 0)
                {
                    // to save proccessing power check which field has the smalles amount of possible values
                    int amountOfPossibleValues = GetAmountOfPossibleValues(sudokuObject, i, j);
                    if (amountOfPossibleValues < smallestAmountOfPossibleValues)
                    {
                        smallestAmountOfPossibleValues = amountOfPossibleValues;
                        row = i; 
                        column = j;
                    }
                }
            }
        }

        if (onlyOne)
        {
            if (smallestAmountOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        if (smallestAmountOfPossibleValues < 10)
        {
            // i found a field where i can put in only one value
            return true;
        }

        return false;
    }

    private static int GetAmountOfPossibleValues(SudokuObject sudokuObject, int row, int column)
    {
        int amount = 0;

        for (int k = 1; k < 10; k++)
        {
            if (sudokuObject.IsPossibleNumberInPosition(k, row, column))
            {
                // field is empty and number is found
                amount++;
            }
        }

        return amount;
    }
    private static List<int> GetPossibleValues(SudokuObject sudokuObject, int row, int column)
    {
        List<int> possbileValues = new();

        for (int value = 1; value < 10; value++)
        {
            if (sudokuObject.IsPossibleNumberInPosition(value, row, column))
            {
                possbileValues.Add(value);
            }
        }

        return possbileValues;
    }

    private static void InsertRandomGroups(SudokuObject sudokuObject)
    {
        List<int> values = new() { 0, 1, 2 };

        var index = UnityEngine.Random.Range(0, values.Count);
        InsertGroup(sudokuObject, 1 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertGroup(sudokuObject, 4 + values[index]);
        values.RemoveAt(index);

        index = UnityEngine.Random.Range(0, values.Count);
        InsertGroup(sudokuObject, 7 + values[index]);
    }
    private static void InsertGroup(SudokuObject sudokuObject, int group)
    {
        List<int> values = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        sudokuObject.GetIndexOfGroup(group, out int startRow, out int startColumn);

        for (int row = startRow; row < startRow +3 ; row++)
        {
            for (int column = startColumn; column < startColumn + 3; column++)
            {
                int index = UnityEngine.Random.Range(0, values.Count);
                sudokuObject.Values[row, column] = values[index];
                values.RemoveAt(index);
            }
        }
    }
}
