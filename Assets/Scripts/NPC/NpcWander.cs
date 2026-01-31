using UnityEngine;
using UnityEngine.AI; // NavMesh için şart!

[RequireComponent(typeof(NavMeshAgent))]
public class NpcWander : MonoBehaviour
{
    [Header("Gezinme Ayarları")]
    public float yurumeYaricapi = 10f; // Ne kadar uzağa gidebilir?
    public float beklemeSuresi = 2f; // Vardığı yerde kaç sn beklesin?

    private NavMeshAgent agent;
    private float timer;
    private bool hareketEdebilir = true; // Konuşurken durdurmak için

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = beklemeSuresi;
        agent = GetComponent<NavMeshAgent>();
    
    // YENİ: Herkese rastgele bir öncelik ver ki birbirlerini itmesinler
    // Düşük sayı = Yüksek öncelik (Yol onun hakkı)
    agent.avoidancePriority = Random.Range(30, 70);
    }

    void Update()
    {
        // Eğer konuşuyorsak hareket etme
        if (!hareketEdebilir) return;

        // Zamanlayıcıyı çalıştır
        timer += Time.deltaTime;

        // Bekleme süresi dolduysa yeni bir rota çiz
        if (timer >= beklemeSuresi)
        {
            Vector3 yeniHedef = RandomNavSphere(transform.position, yurumeYaricapi, -1);
            agent.SetDestination(yeniHedef);
            timer = 0;
        }
    }

    // NavMesh üzerinde rastgele gidilebilir bir nokta bulan fonksiyon
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    // --- DIŞARIDAN KONTROL (Konuşma Başlayınca) ---
    
    public void Dur()
    {
        hareketEdebilir = false;
        if(agent.isOnNavMesh) agent.isStopped = true; // Yürümeyi kes
    }

    public void Yuru()
    {
        hareketEdebilir = true;
        if(agent.isOnNavMesh) agent.isStopped = false; // Devam et
    }
}