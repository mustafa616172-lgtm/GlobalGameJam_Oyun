using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 3f;
    
    // Şu an konuştuğumuz kişiyi hafızada tutalım
    private NpcDialogue suankiNpc; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // DURUM 1: Zaten biriyle konuşuyorsak, konuşmayı bitir (Toggle Kapat)
            if (suankiNpc != null && suankiNpc.dialoguePanel.activeSelf)
            {
                suankiNpc.EtkilesimYap(); // Kapatır
                suankiNpc = null; // Hafızayı temizle
                return; // Aşağıdaki koda inme, işlemi bitir
            }

            // DURUM 2: Kimseyle konuşmuyorsak, önümüzdekine bak (Raycast)
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                NpcDialogue npc = hit.collider.GetComponent<NpcDialogue>();
                if (npc != null)
                {
                    suankiNpc = npc; // Bu kişiyi hafızaya al
                    npc.EtkilesimYap(); // Konuşmayı başlat
                }
            }
        }
    }

    // Buton için kullanılan fonksiyon
    public void EkrandakiKisiyiSucla()
    {
        if (suankiNpc != null)
        {
            suankiNpc.BuKisiyiSucla();
            suankiNpc = null; // Suçlayınca ilişkiyi kes
        }
    }
}