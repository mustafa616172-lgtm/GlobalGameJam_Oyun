using UnityEngine;

public class NpcSimpleMove : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float yurumeHizi = 2.0f;
    public float donusHizi = 5.0f;
    public float beklemeSuresi = 3.0f;

    [Header("Animasyon Ayarı")]
    public string yurumeAnimParametreAdi = "IsWalking"; // Animator'daki bool parametresinin adı

    [Header("Rotalar (Duraklar)")]
    public Transform[] duraklar;

    private int suankiDurakIndex = 0;
    private float beklemeSayaci;
    private bool hareketEdebilir = true;
    private bool bekliyorMu = false;

    private Animator anim; // Animasyon kontrolcüsü

    void Start()
    {
        anim = GetComponent<Animator>(); // Animator'ı otomatik bul
        
        // Güvenlik: Başlangıçta ilk durağa dönsün
        if (duraklar.Length > 0)
        {
            Vector3 hedef = duraklar[0].position;
            transform.LookAt(new Vector3(hedef.x, transform.position.y, hedef.z));
        }
    }

    void Update()
    {
        // 1. Konuşuyorsak hareket etme ve animasyonu durdur
        if (!hareketEdebilir)
        {
            AnimasyonAyarla(false);
            return;
        }

        if (duraklar.Length == 0) return;

        // 2. Hedefe Mesafe Kontrolü
        Transform hedef = duraklar[suankiDurakIndex];
        float mesafe = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), 
                                      new Vector3(hedef.position.x, 0, hedef.position.z));

        if (mesafe < 0.5f) // Hedefe vardık
        {
            AnimasyonAyarla(false); // Durduğu için Idle animasyonuna geç

            if (!bekliyorMu)
            {
                bekliyorMu = true;
                beklemeSayaci = beklemeSuresi;
            }
            
            beklemeSayaci -= Time.deltaTime;
            
            if (beklemeSayaci <= 0)
            {
                suankiDurakIndex = (suankiDurakIndex + 1) % duraklar.Length; // Sonraki durak
                bekliyorMu = false;
            }
        }
        else // Yürüyoruz
        {
            AnimasyonAyarla(true); // Walk animasyonuna geç

            // a) Yüzünü Dön
            Vector3 yon = (hedef.position - transform.position).normalized;
            yon.y = 0; // Havaya bakmasın
            if (yon != Vector3.zero)
            {
                Quaternion hedefRotasyon = Quaternion.LookRotation(yon);
                transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, donusHizi * Time.deltaTime);
            }

            // b) İleri Git
            transform.Translate(Vector3.forward * yurumeHizi * Time.deltaTime);
        }
    }

    void AnimasyonAyarla(bool yuruyorMu)
    {
        if (anim != null)
        {
            anim.SetBool(yurumeAnimParametreAdi, yuruyorMu);
        }
    }

    // --- NpcDialogue Tarafından Çağrılanlar ---
    public void Dur()
    {
        hareketEdebilir = false;
        AnimasyonAyarla(false); // Konuşunca animasyon dursun (Idle)
    }

    public void Yuru()
    {
        hareketEdebilir = true;
    }
}