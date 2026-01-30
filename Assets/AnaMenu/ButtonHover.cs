using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Mouse olaylarını algılamak için gerekli
using TMPro; // TextMeshPro kullanıyorsan gerekli

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Görsel Ayarlar")]
    public TextMeshProUGUI buttonText; // Eğer normal Text kullanıyorsan burayı 'Text' olarak değiştir
    public Color normalColor = Color.gray; // Normal hali (Örn: Gri)
    public Color hoverColor = Color.white; // Üzerine gelinceki renk (Örn: Beyaz/Parlak)
    
    [Range(1f, 1.5f)]
    public float hoverScale = 1.1f; // Üzerine gelince ne kadar büyüsün? (1.1 = %10 büyüme)
    public bool enableGlowEffect = true; // Materyal glow'unu açıp kapatmak ister misin?

    [Header("Ses Ayarları")]
    public AudioSource audioSource; // Sesin çıkacağı kaynak
    public AudioClip hoverSound;    // Üzerine gelme sesi (bip, hışırtı vs.)
    public AudioClip clickSound;    // Tıklama sesi

    private Vector3 initialScale;

    void Start()
    {
        // Başlangıç boyutunu ve rengini kaydet
        if (buttonText == null) 
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        initialScale = transform.localScale;
        
        // Başlangıçta normal renge döndür
        if (buttonText != null)
            buttonText.color = normalColor;
            
        // Eğer materyal glow kullanıyorsan başlangıçta kapat
        if (enableGlowEffect && buttonText != null)
            buttonText.fontSharedMaterial.EnableKeyword(ShaderUtilities.Keyword_Glow);
    }

    // Mouse butonun üzerine geldiğinde çalışır
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1. Yazı Rengini Değiştir (Parlaklık hissi)
        if (buttonText != null)
            buttonText.color = hoverColor;

        // 2. Butonu Hafifçe Büyüt
        transform.localScale = initialScale * hoverScale;

        // 3. (Opsiyonel) TMP Glow Efektini aç (Neon etkisi için)
        if (enableGlowEffect && buttonText != null)
            buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.5f); // Parlaklığı artır

        // 4. Sesi Çal
        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    // Mouse butonun üzerinden gittiğinde çalışır
    public void OnPointerExit(PointerEventData eventData)
    {
        // Her şeyi eski haline getir
        if (buttonText != null)
            buttonText.color = normalColor;

        transform.localScale = initialScale;
        
        // Glow efektini sıfırla
        if (enableGlowEffect && buttonText != null)
            buttonText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
    }

    // Butona tıklandığında çalışır
    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
            
        // Burada oyun başlatma kodu da çağrılabilir ama onu ayrı tutmak daha temizdir.
    }
}