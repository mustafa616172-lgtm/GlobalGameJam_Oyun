using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Sahne Ayarları")]
    public string gameSceneName = "SampleScene"; // Oyunun ana sahnesinin adı
    public GameObject optionsPanel; // Ayarlar penceresi (Panel)

    // Kayıt anahtarı (Veritabanındaki değişken ismi gibi düşün)
    private const string SAVE_KEY = "HasSavedGame"; 

    // 1. YENİ OYUN BUTONU
    public void NewGame()
    {
        // 1 = Hikayeyi Göster demektir
        PlayerPrefs.SetInt("ShowIntroStory", 1); 
        
        // Yeni oyun olduğu için kayıt var mı bilgisini oluştur
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("HasSavedGame"))
        {
            // Devam ettiği için hikayeyi GÖSTERME (0)
            PlayerPrefs.SetInt("ShowIntroStory", 0); 
            
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            // Kayıt yoksa yeni oyun gibi davran
            NewGame();
        }
    }

    // 3. AYARLAR BUTONU
    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true); // Paneli aç
        }
    }

    public void CloseOptions() // Ayarlar panelindeki 'Geri' butonu için
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false); // Paneli kapat
        }
    }

    // 4. ÇIKIŞ BUTONU
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkıldı!");
        Application.Quit();
    }
}