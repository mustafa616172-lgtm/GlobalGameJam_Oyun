using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NpcDialogue : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject suclaButonu;

    [Header("Otomatik Dolacaklar")]
    public string npcMesaji;
    public bool buKisiKatilMi = false;

    // Kapatıp açacağımız scriptlerin referansları
    private MouseLook mouseLookScript;
    private Player3DMovement movementScript; // Senin hareket scriptin adı neyse o

    void Start()
    {
        // Oyuncuyu bul ve scriptlerine eriş (Tag'in "Player" olduğundan emin ol!)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Scriptler Player'ın üzerindeyse GetComponent, 
            // Kameranın üzerindeyse (MouseLook gibi) GetComponentInChildren kullanabiliriz.
            // En garantisi Main Camera'yı bulmaktır:
            mouseLookScript = Camera.main.GetComponent<MouseLook>();
            movementScript = player.GetComponent<Player3DMovement>();
        }
    }

    public void EtkilesimYap()
    {
        bool panelAcikMi = dialoguePanel.activeSelf;

        if (!panelAcikMi)
        {
            // --- AÇILIRKEN ---
            dialoguePanel.SetActive(true);
            dialogueText.text = npcMesaji;
            suclaButonu.SetActive(true);

            // 1. Mouse'u serbest bırak
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 2. Arka plandaki kamera kontrolünü DURDUR
            if (mouseLookScript != null) mouseLookScript.enabled = false;
            if (movementScript != null) movementScript.enabled = false;
        }
        else
        {
            Kapat();
        }
    }

    public void Kapat()
    {
        // --- KAPANIRKEN ---
        dialoguePanel.SetActive(false);
        suclaButonu.SetActive(false);

        // 1. Mouse'u tekrar kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. Kontrolleri geri AÇ
        if (mouseLookScript != null) mouseLookScript.enabled = true;
        if (movementScript != null) movementScript.enabled = true;
    }

    public void BuKisiyiSucla()
    {
        if (buKisiKatilMi)
        {
            Debug.Log("KAZANDIN!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("KAYBETTİN! Yanlış kişi.");
        }
        Kapat();
    }
}