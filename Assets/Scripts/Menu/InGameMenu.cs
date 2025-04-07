using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
