using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class NpcDialogue : MonoBehaviour
{
    [Header("UI Bağlantıları (Inspector'dan Sürükle)")]
    public GameObject dialoguePanel;      
    public TextMeshProUGUI dialogueText;  
    public TextMeshProUGUI nameTextUI;    
    public GameObject suclaButonu;        

    [Header("Otomatik Dolacaklar")]
    public string npcAdi;                 
    public string npcMesaji;              
    public bool buKisiKatilMi = false;    

    // --- Referanslar ---
    private MouseLook mouseLookScript;       
    private Player3DMovement movementScript; 
    
    // YENİ: NavMesh yerine basit hareket scriptini kullanıyoruz
    private NpcSimpleMove simpleMoveScript;          
    
    private Transform playerTransform;       
    private bool konusuyorMuyuz = false;     

    void Start()
    {
        // 1. Kendi üzerindeki YENİ hareket scriptini bul
        simpleMoveScript = GetComponent<NpcSimpleMove>();

        // 2. Oyuncuyu bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            mouseLookScript = Camera.main.GetComponent<MouseLook>();
            movementScript = player.GetComponent<Player3DMovement>();
        }
    }

    void Update()
    {
        // Konuşurken sürekli oyuncuya dön
        if (konusuyorMuyuz && playerTransform != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(targetPosition);
        }
    }

    public void EtkilesimYap()
    {
        if (!konusuyorMuyuz) Ac();
        else Kapat();
    }

    void Ac()
    {
        konusuyorMuyuz = true;

        // UI Aç
        dialoguePanel.SetActive(true);
        dialogueText.text = npcMesaji;
        
        if (nameTextUI != null) nameTextUI.text = npcAdi;
        
        suclaButonu.SetActive(true);

        // Mouse Serbest
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Oyuncuyu Dondur
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (movementScript != null) movementScript.enabled = false;
        
        // NPC'yi Durdur (Animasyonu rölantiye alacak)
        if (simpleMoveScript != null) simpleMoveScript.Dur();                 
    }

    public void Kapat()
    {
        konusuyorMuyuz = false;

        // UI Kapat
        dialoguePanel.SetActive(false);
        suclaButonu.SetActive(false);

        // Mouse Kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Oyuncuyu Aç
        if (mouseLookScript != null) mouseLookScript.enabled = true;
        if (movementScript != null) movementScript.enabled = true;
        
        // NPC Yürüsün (Animasyon tekrar başlayacak)
        if (simpleMoveScript != null) simpleMoveScript.Yuru(); 
    }

    public void BuKisiyiSucla()
    {
        if (buKisiKatilMi)
        {
            Debug.Log("TEBRİKLER! KATİLİ YAKALADIN!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("MASUM BİRİNİ SUÇLADIN!");
        }
        Kapat();
    }
}