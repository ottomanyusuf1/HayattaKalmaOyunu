using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("GameScane");
    }
    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
