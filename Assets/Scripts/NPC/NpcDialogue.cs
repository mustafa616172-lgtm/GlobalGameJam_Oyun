using UnityEngine;
using TMPro; // TextMeshPro kullanıyorsan bu kütüphane şart

public class NpcDialogue : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public GameObject dialoguePanel; // Az önce yarattığın Panel
    public TextMeshProUGUI dialogueText; // Panelin içindeki Text

    [Header("Diyalog Ayarları")]
    [TextArea] public string npcMesaji = "Merhaba yabancı! Global Game Jam nasıl gidiyor?";
    
    private bool isDialogueOpen = false;

    public void EtkilesimYap()
    {
        if (!isDialogueOpen)
        {
            // Diyaloğu Aç
            dialoguePanel.SetActive(true);
            dialogueText.text = npcMesaji;
            isDialogueOpen = true;
            
            // Mouse'u geri getir (ki yazıyı okurken rahat olsun, opsiyonel)
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Diyaloğu Kapat
            dialoguePanel.SetActive(false);
            isDialogueOpen = false;
            
            // Mouse'u tekrar kilitle
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}