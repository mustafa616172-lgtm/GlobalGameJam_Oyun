using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Coroutine için gerekli

public class MainMenuManager : MonoBehaviour
{
    [Header("Sahne Ayarları")]
    public string gameSceneName = "GameScene";
    public GameObject optionsPanel;

    [Header("Fade Ayarları")]
    public CanvasGroup fadeCanvasGroup; // Siyah panelin CanvasGroup'u
    public float fadeDuration = 1.0f; // Kararma süresi

    private const string SAVE_KEY = "HasSavedGame"; 

    // --- YENİ OYUN ---
    public void NewGame()
    {
        // Önce verileri ayarla
        PlayerPrefs.SetInt("ShowIntroStory", 1); 
        PlayerPrefs.SetInt(SAVE_KEY, 1);
        PlayerPrefs.Save();

        // Direkt yüklemek yerine Coroutine başlat
        StartCoroutine(FadeAndLoadScene());
    }

    // --- DEVAM ET ---
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.SetInt("ShowIntroStory", 0);
            StartCoroutine(FadeAndLoadScene()); // Fade ile geçiş
        }
        else
        {
            NewGame(); // Kayıt yoksa NewGame fonksiyonuna gider
        }
    }

    // --- FADE OUT VE SAHNE YÜKLEME ---
    IEnumerator FadeAndLoadScene()
    {
        // 1. Fade Panelinin tıklamaları engellemesini sağla (Blocks Raycasts)
        fadeCanvasGroup.blocksRaycasts = true;

        // 2. Alpha değerini 0'dan 1'e (Siyah) çıkar
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / fadeDuration;
            yield return null; // Bir sonraki kareyi bekle
        }

        fadeCanvasGroup.alpha = 1f; // Tam siyah olduğundan emin ol

        // 3. Sahneyi Yükle
        SceneManager.LoadScene(gameSceneName);
    }

    // --- DİĞER FONKSİYONLAR (Aynı kalacak) ---
    public void OpenOptions() { optionsPanel.SetActive(true); }
    public void CloseOptions() { optionsPanel.SetActive(false); }
    public void QuitGame() { Application.Quit(); }
}