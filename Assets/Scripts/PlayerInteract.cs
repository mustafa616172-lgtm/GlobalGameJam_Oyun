using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 3f;
    private NpcDialogue suAnKonusulanNpc; // Kimi dinliyoruz?

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                NpcDialogue npc = hit.collider.GetComponent<NpcDialogue>();
                if (npc != null)
                {
                    suAnKonusulanNpc = npc; // Hafızaya al
                    npc.EtkilesimYap();
                }
            }
        }
    }

    // BUTONA TIKLAYINCA BU FONKSİYONU ÇAĞIRACAĞIZ
    public void EkrandakiKisiyiSucla()
    {
        if (suAnKonusulanNpc != null)
        {
            suAnKonusulanNpc.BuKisiyiSucla();
        }
    }
}