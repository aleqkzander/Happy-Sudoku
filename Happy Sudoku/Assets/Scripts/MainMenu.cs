using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartEasyGame()
    {
        SceneManager.LoadScene(1);
        GameSettings.difficutlyLevel = 1;
    }

    public void StartMiddleGame()
    {
        SceneManager.LoadScene(1);
        GameSettings.difficutlyLevel = 2;
    }

    public void StartHardGame()
    {
        SceneManager.LoadScene(1);
        GameSettings.difficutlyLevel = 3;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StopGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit game");
    }
}
