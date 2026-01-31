using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Referansları")]
    public GameObject settingsPanel;      // Ayarlar menüsünün ana paneli (Aç/Kapa için)
    public Button saveButton;             // Kaydet butonu (Rengi değişecek)
    public Button backButton;             // Geri butonu
    
    [Header("Ayarlar UI")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public TMP_Dropdown graphicsDropdown;

    [Header("Sistem Referansları")]
    public AudioMixer mainMixer;          // Ses kontrolü için
    
    [Header("Renk Ayarları")]
    public Color unsavedColor = Color.red;   // Kaydedilmemiş renk (Kırmızı)
    public Color savedColor = Color.green;   // Kaydedilmiş renk (Yeşil)

    // --- DEĞİŞKENLER (Hafıza) ---
    // Oyuncu menüyü açtığında var olan gerçek değerleri burada tutacağız.
    // Eğer kaydetmeden çıkarsa, bu değerlere geri döneceğiz.
    private float _startVolume;
    private float _startSensitivity;
    private int _startQuality;

    private bool _isDirty = false; // Değişiklik yapıldı mı?

    // PlayerPrefs Anahtarları
    private const string KEY_VOLUME = "Volume";
    private const string KEY_SENSITIVITY = "Sensitivity";
    private const string KEY_QUALITY = "Quality";

    void Start()
    {
        // Butonlara tıklama olaylarını bağla
        saveButton.onClick.AddListener(SaveSettings);
        backButton.onClick.AddListener(OnBackButtonClicked);

        // Değerler değiştiğinde çalışacak fonksiyonları bağla
        volumeSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        sensitivitySlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        graphicsDropdown.onValueChanged.AddListener(delegate { OnValueChanged(); });

        // Oyunu başlatırken veya menü açılırken ayarları yükle
        LoadCurrentSettings();
    }

    // Menü her açıldığında (SetActive true olduğunda) çalışır
    private void OnEnable()
    {
        LoadCurrentSettings();
    }

    // 1. MEVCUT AYARLARI YÜKLE VE HAFIZAYA AL
    public void LoadCurrentSettings()
    {
        // A) PlayerPrefs'ten verileri çek (Yoksa varsayılanları al)
        _startVolume = PlayerPrefs.GetFloat(KEY_VOLUME, 0.5f);
        _startSensitivity = PlayerPrefs.GetFloat(KEY_SENSITIVITY, 1.0f);
        _startQuality = PlayerPrefs.GetInt(KEY_QUALITY, 2); // Medium

        // B) UI Elemanlarını güncelle
        volumeSlider.value = _startVolume;
        sensitivitySlider.value = _startSensitivity;
        graphicsDropdown.value = _startQuality;

        // C) Arka planda sistemi güncelle (Ses, Grafik vb.)
        ApplySettings(_startVolume, _startSensitivity, _startQuality);

        // D) Buton rengini Yeşil yap (Henüz değişiklik yok)
        UpdateSaveButtonColor(true);
        _isDirty = false;
    }

    // 2. HERHANGİ BİR AYAR DEĞİŞTİĞİNDE
    public void OnValueChanged()
    {
        // Anlık olarak sesi veya grafiği değiştirip göstermek istersen burayı açabilirsin.
        // Ama "Kaydetmezse iptal olsun" dediğin için sadece UI değişiyor, sistem henüz değişmiyor.
        // İstersen "Preview" (Önizleme) için ApplySettings'i burada çağırabilirsin ama 
        // Back tuşunda RevertSettings çağırmak şartıyla. 
        
        // Biz şimdilik kullanıcıya değişikliği göstermek için "Preview" yapalım:
        ApplySettings(volumeSlider.value, sensitivitySlider.value, graphicsDropdown.value);

        // Kaydet butonu Kırmızı olsun
        UpdateSaveButtonColor(false);
        _isDirty = true;
    }

    // 3. KAYDET BUTONUNA BASINCA
    public void SaveSettings()
    {
        // Verileri PlayerPrefs'e yaz (Diske kaydet)
        PlayerPrefs.SetFloat(KEY_VOLUME, volumeSlider.value);
        PlayerPrefs.SetFloat(KEY_SENSITIVITY, sensitivitySlider.value);
        PlayerPrefs.SetInt(KEY_QUALITY, graphicsDropdown.value);
        PlayerPrefs.Save();

        // Başlangıç noktalarımızı (Restore noktalarını) güncelle
        _startVolume = volumeSlider.value;
        _startSensitivity = sensitivitySlider.value;
        _startQuality = graphicsDropdown.value;

        // Butonu Yeşil yap
        UpdateSaveButtonColor(true);
        _isDirty = false;

        Debug.Log("Ayarlar Kaydedildi!");
    }

    // 4. GERİ BUTONUNA BASINCA
    public void OnBackButtonClicked()
    {
        if (_isDirty)
        {
            // Eğer değişiklik yapıldı ama KAYDEDİLMEDİYSE
            // Eski (Başlangıç) ayarlarına geri dön (REVERT)
            ApplySettings(_startVolume, _startSensitivity, _startQuality);
            
            // UI'ı da eski haline getir (Bir sonraki açılışta düzgün görünsün)
            volumeSlider.value = _startVolume;
            sensitivitySlider.value = _startSensitivity;
            graphicsDropdown.value = _startQuality;

            Debug.Log("Değişiklikler kaydedilmediği için geri alındı.");
        }

        // Paneli kapat
        settingsPanel.SetActive(false);
        
        // Oyun içindeysek Time.timeScale = 1 yapman gerekebilir (Pause'dan çıkış için)
    }

    // --- YARDIMCI FONKSİYONLAR ---

    // Ayarları motora uygulayan fonksiyon
    private void ApplySettings(float vol, float sens, int quality)
{
    // 1. Ses Ayarı (Aynı kalıyor)
    float dbValue = Mathf.Log10(Mathf.Max(vol, 0.0001f)) * 20;
    if(mainMixer) mainMixer.SetFloat("MusicVol", dbValue);

    // 2. Grafik Ayarı (Aynı kalıyor)
    QualitySettings.SetQualityLevel(quality);

    // --- YENİ EKLENEN KISIM: HASSASİYET ---
    
    // Sahne içinde aktif olan MouseLook scriptini bulmaya çalış
    // (FindFirstObjectByType Unity 2023 ve sonrası için, eski sürümse FindObjectOfType kullan)
    MouseLook playerScript = FindFirstObjectByType<MouseLook>();

    // Eğer oyun sahnesindeysek (playerScript varsa) hassasiyeti anlık güncelle
    if (playerScript != null)
    {
        playerScript.mouseSensitivity = sens;
    }
    
    // Eğer Ana Menüdeysek playerScript 'null' döner, kod hata vermez, sadece çalışmaz.
    // Bu tam istediğimiz şey çünkü menüde dönecek karakter yok.
}

    // Buton rengini değiştiren fonksiyon
    private void UpdateSaveButtonColor(bool isSaved)
    {
        Image btnImage = saveButton.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = isSaved ? savedColor : unsavedColor;
        }
    }
}