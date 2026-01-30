using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    void Start()
    {
        // Başlangıçta ekran tam siyah olsun
        fadeCanvasGroup.alpha = 1f;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // 1'den 0'a doğru git (Siyah -> Şeffaf)
            fadeCanvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false; // Tıklamaları engellemesin
        
        // İstersen bu objeyi yok edebilirsin, gerek kalmadı
        // Destroy(gameObject); 
    }
}