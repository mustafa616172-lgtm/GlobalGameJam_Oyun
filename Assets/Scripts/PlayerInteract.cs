using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 3f; // Ne kadar yakından basabilirsin
    public LayerMask interactionLayer; // Sadece belli layerlarla etkileşim (Opsiyonel)

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);
        // E tuşuna basıldı mı?
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Kameranın ortasından ileriye bir ışın yolla
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Işın bir şeye çarptı mı?
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // Çarptığı şeyde "NpcDialogue" scripti var mı?
                NpcDialogue npc = hit.collider.GetComponent<NpcDialogue>();
                
                if (npc != null)
                {
                    // Varsa etkileşimi başlat!
                    npc.EtkilesimYap();
                }
            }
        }
    }
}