using UnityEngine;
using System.Collections; // Coroutine için gerekli

public class StorySkipManager : MonoBehaviour
{
    [Header("UI Objeleri")]
    public GameObject storyPanel; 
    public GameObject playerObject;
    
    [Header("Fade Ayarları")]
    public CanvasGroup fadeCanvasGroup; // Siyah Perdemiz
    public float transitionDuration = 1.0f; // Kararma süresi

    private bool isStoryActive = false;
    private bool isTransitioning = false; // Geçiş sırasında tekrar basılmasın diye

    void Start()
    {
        // Ana menüden gelen emri kontrol et
        int showStory = PlayerPrefs.GetInt("ShowIntroStory", 1);

        if (showStory == 1)
        {
            ShowStory();
        }
        else
        {
            // Hikaye yoksa direkt karakteri aç (SceneFadeIn scripti zaten açılış fade'ini yapar)
            QuickStart();
        }
    }

    void Update()
    {
        // Hikaye açıksa, geçiş yapılmıyorsa ve E'ye basıldıysa
        if (isStoryActive && !isTransitioning)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(TransitionToGame());
            }
        }
    }

    void ShowStory()
    {
        isStoryActive = true;
        storyPanel.SetActive(true);
        if (playerObject != null) playerObject.SetActive(false);
    }

    void QuickStart()
    {
        isStoryActive = false;
        storyPanel.SetActive(false);
        if (playerObject != null) playerObject.SetActive(true);
    }

    // --- SİHİRLİ KISIM BURASI ---
    IEnumerator TransitionToGame()
    {
        isTransitioning = true; // Oyuncu tekrar E'ye basamasın

        // 1. ADIM: EKRANI KARART (Fade Out)
        float timer = 0f;
        fadeCanvasGroup.blocksRaycasts = true; // Tıklamayı engelle

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = timer / transitionDuration; // 0'dan 1'e git
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f; // Tam siyah

        // 2. ADIM: SAHNEYİ DEĞİŞTİR (Karanlıkta değişim)
        storyPanel.SetActive(false); // Hikayeyi kapat
        if (playerObject != null) playerObject.SetActive(true); // Karakteri aç

        yield return new WaitForSeconds(0.5f); // Simsiyah ekranda yarım saniye bekle (Gerilim için iyi)

        // 3. ADIM: EKRANI AYDINLAT (Fade In)
        timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (timer / transitionDuration); // 1'den 0'a git
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f; // Tam şeffaf
        fadeCanvasGroup.blocksRaycasts = false;

        isStoryActive = false;
    }
}