using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // Oyunu tekrar başlatmak için zamanı geri aç
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Mevcut sahneyi yeniden yükle
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Menüye dönmeden önce zamanı geri aç
        SceneManager.LoadScene("MainMenu"); // Menü sahnenin ismi buysa
    }
}
