using System.Collections;
using TMPro;
using UnityEditor.Timeline.Actions;
using UnityEngine;


public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }
    public int dayInGame= 1;
    public TextMeshProUGUI dayUI;
    public GameObject gameOverScreen;
    private CanvasGroup canvasGroup;
    


    private void Awake()
    
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
  

    void Start()
    {
        dayUI.text = $"Day: {dayInGame}";
        canvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
        gameOverScreen.SetActive(false);
    }

    public void TriggerNextDay()
    {
        dayInGame += 1;
        dayUI.text = $"Day: {dayInGame}";

        if (dayInGame >= 6)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f; // Oyunu durdur
        Cursor.visible = true; // Fareyi görünür yap
        Cursor.lockState = CursorLockMode.None;  // Fareyi serbest bırak

        

        gameOverScreen.SetActive(true); // Game Over ekranını aç
        StartCoroutine(FadeInGameOver());
    }
    
    private IEnumerator FadeInGameOver()
    {
        float duration = 1.5f;
        float currentTime = 0f;
        canvasGroup.alpha = 0f;

        while (currentTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
}
