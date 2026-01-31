using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    // HASSASİYET GLOBAL ERİŞİM (Karakterin buradan okuyacak)
    public static float GlobalSensitivity = 1.0f;

    [Header("UI Referansları")]
    public GameObject settingsPanel;      // Ayarlar penceresi
    public Button saveButton;             // Kaydet butonu
    public Button backButton;             // Geri butonu

    [Header("Ayarlar UI Elemanları")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public TMP_Dropdown graphicsDropdown; // Sadece Düşük-Orta-Yüksek seçenekli

    [Header("Sistem Referansları")]
    public AudioMixer mainMixer;          // Ses kontrolü (MusicVol parametresi)

    [Header("Renk Ayarları")]
    public Color unsavedColor = Color.red;   // Kaydedilmedi uyarısı (Kırmızı)
    public Color savedColor = Color.green;   // Kaydedildi (Yeşil)

    // --- HAFIZA (GERİ ALMA İÇİN) ---
    private float _startVolume;
    private float _startSensitivity;
    private int _startQuality; // 0: Düşük, 1: Orta, 2: Yüksek

    private bool _isDirty = false; // Değişiklik yapıldı mı kontrolü

    // Kayıt Anahtarları
    private const string KEY_VOLUME = "Volume";
    private const string KEY_SENSITIVITY = "Sensitivity";
    private const string KEY_QUALITY = "Quality";

    void Start()
    {
        // 1. Butonları Bağla
        saveButton.onClick.AddListener(SaveSettings);
        backButton.onClick.AddListener(OnBackButtonClicked);

        // 2. Değişiklikleri Dinle (Anlık önizleme için)
        volumeSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        graphicsDropdown.onValueChanged.AddListener(delegate { OnValueChanged(); });

        // 3. Ayarları Yükle
        LoadCurrentSettings();
    }

    private void OnEnable()
    {
        // Menü her açıldığında verileri tazele
        LoadCurrentSettings();
    }

    // --- MEVCUT AYARLARI YÜKLE ---
    public void LoadCurrentSettings()
    {
        // Verileri çek (Varsayılan değerler parantez içinde)
        _startVolume = PlayerPrefs.GetFloat(KEY_VOLUME, 0.5f);
        _startSensitivity = PlayerPrefs.GetFloat(KEY_SENSITIVITY, 2.0f);
        _startQuality = PlayerPrefs.GetInt(KEY_QUALITY, 2); // Varsayılan: Yüksek (2)

        // UI elemanlarını güncelle
        volumeSlider.value = _startVolume;
        sensitivitySlider.value = _startSensitivity;
        graphicsDropdown.value = _startQuality;

        // Arka planda oyuna uygula
        ApplySettings(_startVolume, _startSensitivity, _startQuality);

        UpdateSaveButtonColor(true);
        _isDirty = false;
    }

    // --- BİR ŞEY DEĞİŞİNCE ÇALIŞIR ---
    public void OnValueChanged()
    {
        // Anlık olarak uygula (Preview) ama kaydetme
        ApplySettings(volumeSlider.value, sensitivitySlider.value, graphicsDropdown.value);
        
        UpdateSaveButtonColor(false); // Buton Kırmızı olsun
        _isDirty = true;
    }

    // --- KAYDET BUTONU ---
    public void SaveSettings()
    {
        // Diske yaz
        PlayerPrefs.SetFloat(KEY_VOLUME, volumeSlider.value);
        PlayerPrefs.SetFloat(KEY_SENSITIVITY, sensitivitySlider.value);
        PlayerPrefs.SetInt(KEY_QUALITY, graphicsDropdown.value);
        PlayerPrefs.Save();

        // Geri dönüş noktalarımızı güncelle (Artık yeni başlangıç noktası burası)
        _startVolume = volumeSlider.value;
        _startSensitivity = sensitivitySlider.value;
        _startQuality = graphicsDropdown.value;

        UpdateSaveButtonColor(true); // Buton Yeşil olsun
        _isDirty = false;
        
        Debug.Log("Ayarlar Kaydedildi!");
    }

    // --- GERİ BUTONU (REVERT İŞLEMİ) ---
    public void OnBackButtonClicked()
    {
        if (_isDirty)
        {
            // Eğer değişiklik yapıldı ama KAYDEDİLMEDİYSE -> İPTAL ET (Eski ayarlara dön)
            ApplySettings(_startVolume, _startSensitivity, _startQuality);
            
            // UI'ı da eski haline getir ki sonraki açılışta doğru gözüksün
            volumeSlider.value = _startVolume;
            sensitivitySlider.value = _startSensitivity;
            graphicsDropdown.value = _startQuality;

            Debug.Log("Değişiklikler kaydedilmedi, eski ayarlara dönüldü.");
        }

        // Paneli kapat
        settingsPanel.SetActive(false);
    }

    // --- AYARLARI UYGULAMA MOTORU ---
    private void ApplySettings(float vol, float sens, int quality)
    {
        // 1. Ses
        // Not: AudioMixer kullanmıyorsan burayı silebilirsin veya AudioListener.volume = vol; yapabilirsin.
        float dbValue = Mathf.Log10(Mathf.Max(vol, 0.0001f)) * 20;
        if (mainMixer) mainMixer.SetFloat("MusicVol", dbValue);

        // 2. Grafik Kalitesi (0: Düşük, 1: Orta, 2: Yüksek)
        QualitySettings.SetQualityLevel(quality);

        // 3. Hassasiyet (Global değişkene yaz)
        GlobalSensitivity = sens;
    }

    // Buton Renk Değişimi
    private void UpdateSaveButtonColor(bool isSaved)
    {
        Image btnImage = saveButton.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = isSaved ? savedColor : unsavedColor;
        }
    }
}