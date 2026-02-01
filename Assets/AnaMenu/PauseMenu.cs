using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("Script Referansları")]
    public SettingsManager settingsManagerScript;
    
    // YENİ: Karakterin kamerasını kontrol eden scripti buraya alacağız
    [Header("Karakter Kontrolü")]
    public MouseLook mouseLookScript; 

    [Header("Sahne Ayarları")]
    public string anaMenuSahneIsmi = "AnaMenu";

    public static bool isGamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        // 1. Panelleri ayarla
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);

        // 2. Zamanı durdur
        Time.timeScale = 0f;
        isGamePaused = true;

        // 3. FAREYİ SERBEST BIRAK VE GÖSTER
        Cursor.lockState = CursorLockMode.None; // Kilidi kaldır
        Cursor.visible = true;                  // Görünür yap

        // 4. KAMERA DÖNMESİNİ ENGELLE (Çok Önemli!)
        if (mouseLookScript != null)
        {
            mouseLookScript.enabled = false; // Scripti tamamen durdurur
        }
    }

    public void ResumeGame()
    {
        // 1. Panelleri kapat
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        // 2. Zamanı devam ettir
        Time.timeScale = 1f;
        isGamePaused = false;

        // 3. FAREYİ KİLİTLE VE GİZLE
        Cursor.lockState = CursorLockMode.Locked; // Ortaya kilitle
        Cursor.visible = false;                   // Gizle

        // 4. KAMERA DÖNMESİNİ TEKRAR AÇ
        if (mouseLookScript != null)
        {
            mouseLookScript.enabled = true; // Script tekrar çalışsın
        }
    }

    // --- DİĞER FONKSİYONLAR (Aynı kalacak) ---
    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        if (settingsManagerScript != null) settingsManagerScript.LoadCurrentSettings();
    }

    public void CloseSettingsAndReturnToPause()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(anaMenuSahneIsmi);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}