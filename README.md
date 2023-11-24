# Happy-Sudoku Discription

Happy-Sudoku is a simple game of sudoku. All sudokus are generated randomly. 
The user can choose between three difficulty settings.

# Note

Difficulty settings can be adjusted in the SudokuGenerator script within the RemoveNumbersFromSudokuObject-Method.
```
        int endOfLoopValue = 10;
        if (GameSettings.difficutlyLevel == 1) endOfLoopValue = 71;
        if (GameSettings.difficutlyLevel == 2) endOfLoopValue = 61;
```
DifficultyLevels can be added in GameSettings.cs.
