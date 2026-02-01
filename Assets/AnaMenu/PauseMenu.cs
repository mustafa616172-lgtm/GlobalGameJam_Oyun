using UnityEngine;
using UnityEngine.SceneManagement; // YENİ: Sahne değiştirmek için bu kütüphane şart!

public class PauseManager : MonoBehaviour
{
    [Header("Ayarlar Paneli")]
    public GameObject settingsPanel; // Prefab'tan gelen panelimiz
    public SettingsManager settingsManagerScript; 

    [Header("Sahne Ayarları")]
    public string anaMenuSahneIsmi = "AnaMenu"; // Buraya Ana Menü sahnennin tam adını yazacaksın!

    [Header("Oyun Durumu")]
    public static bool isGamePaused = false;

    void Update()
    {
        // ESC tuşuna basınca
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        settingsPanel.SetActive(true);
        
        // Ayarları yükle ki en son halleri görünsün
        if(settingsManagerScript != null) 
            settingsManagerScript.LoadCurrentSettings();

        Time.timeScale = 0f; // Zamanı durdur
        isGamePaused = true;

        Cursor.lockState = CursorLockMode.None; // Fareyi serbest bırak
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);

        Time.timeScale = 1f; // Zamanı devam ettir
        isGamePaused = false;

        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitle
        Cursor.visible = false;
    }

    // --- YENİ EKLENEN FONKSİYON ---
    public void AnaMenuyeDon()
    {
        // 1. Çok Önemli: Sahne değişmeden önce zamanı normale döndür.
        // Yoksa Ana Menüye geçtiğinde oyun hala donuk kalır.
        Time.timeScale = 1f;
        isGamePaused = false;

        // 2. Ana Menü sahnesini yükle
        SceneManager.LoadScene(anaMenuSahneIsmi);
    }
}