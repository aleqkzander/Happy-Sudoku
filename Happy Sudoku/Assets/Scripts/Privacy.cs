using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Privacy : MonoBehaviour
{
    public void ShowPrivacyNotice()
    {
        string link = "https://www.alexgamedev.de/";
        Application.OpenURL(link);
    }
}
