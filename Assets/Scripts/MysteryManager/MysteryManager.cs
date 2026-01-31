using UnityEngine;
using System.Collections.Generic;

public class MysteryManager : MonoBehaviour
{
    [System.Serializable]
    public class Supheli
    {
        public string isim; // Örn: "Kovboy"
        public NpcDialogue npcScripti; // Sahnedeki NPC
        [TextArea] public string ipucuTanimi; // Örn: "kırmızı bir atkısı"
    }

    [Header("Şüpheli Listesi")]
    public List<Supheli> tumSupheliler;

    [Header("Olasılık Ayarları")]
    [Range(0f, 1f)] public float gormemeIhtimali = 0.35f; // %35 ihtimalle görmesin
    [Range(0f, 1f)] public float katilinIftiraIhtimali = 0.5f; // %50 ihtimalle suç atsın

    // --- 1. GÖRGÜ TANIKLARI (DOĞRU İPUCU VERENLER) ---
    private string[] masumSablonlari = new string[] 
    {
        "Tam emin değilim ama sanki üzerinde {0} vardı.",
        "Göz ucuyla baktığımda {0} olduğunu fark ettim.",
        "Karanlıktı ama {0} parlıyordu sanki.",
        "Yüzünü saklıyordu ama {0} olduğunu net gördüm.",
        "Çok korkunçtu! Tek hatırladığım {0} olduğu.",
        "Titremekten zor konuşuyorum... Sanırım {0} vardı.",
        "Lütfen beni karıştırma, sadece {0} gördüğümü söyleyebilirim.",
        "Olay yerinden kaçan kişinin {0} dikkatimi çekti.",
        "Polis bey, kaçan kişinin kesinlikle {0} vardı.",
        "Katili arıyorsan {0} olan kişiye dikkat et.",
        "Şu {0} olan tip var ya, bence aradığın kişi o.",
        "İnanamıyorum, az önce {0} olan biri buradan koşarak geçti!",
        "Her şey çok hızlı gelişti... Yine de {0} gözüme çarptı.",
        "Buralarda {0} olan birini gördün mü? O çok şüpheliydi."
    };

    // --- 2. HİÇBİR ŞEY GÖRMEYENLER (BOŞ YAPANLAR) ---
    private string[] bosSablonlar = new string[]
    {
        "Ben o sırada telefona bakıyordum, hiçbir şey görmedim.",
        "Ben buraların yabancısıyım, kim kimdir tanımam.",
        "Kulaklığım takılıydı, dünyadan haberim yok.",
        "Ayakkabımı bağlıyordum, kafamı kaldırdığımda herkes kaçmıştı.",
        "Çok karanlıktı, kimseyi seçemedim maalesef.",
        "Gözlüklerimi evde unutmuşum, her şey bulanıktı.",
        "Kalabalıkta kimseyi seçemedim, üzgünüm.",
        "Olay olduğunda arkam dönüktü.",
        "Korkudan gözlerimi kapattım, hiçbir şey hatırlamıyorum.",
        "Bana soru sorma! Başımın belaya girmesini istemiyorum.",
        "Ben hiçbir şeye karışmam, beni rahat bırak."
    };

    // --- 3. KATİLİN SAVUNMALARI (SUÇU REDDEDENLER) ---
    private string[] katilSavunmalari = new string[] 
    {
        "Ben bütün gün buradaydım memur bey, hiçbir şey görmedim.",
        "Benim olayla bir ilgim yok, sadece işimi yapıyorum.",
        "Burada bir sürü insan var, neden benimle uğraşıyorsun?",
        "Yanlış kişiye soruyorsun, ben sadece bekliyorum.",
        "Neden bana öyle bakıyorsun? Ben masumum!",
        "Beni suçlamaya mı çalışıyorsun? Hadi oradan!",
        "İşine bak dedektif, benden sana ekmek çıkmaz.",
        "Katil mi? Ne katili? Ben sadece hava almaya çıkmıştım.",
        "Hiçbir şey bilmiyorum, lütfen beni rahat bırak.",
        "Ben karıncayı bile incitemem!"
    };

    // --- 4. KATİLİN İFTİRALARI (BAŞKASINI SUÇLAYANLAR) ---
    private string[] katilIftiralari = new string[]
    {
        "Ben yapmadım! Ama koşarak kaçan kişinin {0} vardı!",
        "Olayı şu {0} olan tipin yaptığını söylüyorlar.",
        "Yemin ederim ben değilim, {0} olan kişiden şüpheleniyorum.",
        "Ben masumum ama az önce {0} olan biri buradan geçti.",
        "Aradığın kişi ben değilim, {0} taşıyan kişiye bakmalısın.",
        "Kendi gözlerimle gördüm, {0} olan kişi çok garipti.",
        "Bence zaman kaybediyorsun, git {0} olanı sorgula.",
        "Benim vicdanım rahat. Ama {0} olan kişi çok terlemişti.",
        "Suçlu o tarafa koştu! Hani şu {0} var ya, o işte!"
    };

    void Start()
    {
        RolleriDagit();
    }

    public void RolleriDagit()
    {
        if (tumSupheliler.Count < 2) return;

        // 1. Katili Belirle
        int katilIndex = Random.Range(0, tumSupheliler.Count);
        Supheli katilKisi = tumSupheliler[katilIndex];

        // 2. Günah Keçisi Belirle (Katilin suç atacağı masum kişi)
        int gunahKecisiIndex = katilIndex;
        while (gunahKecisiIndex == katilIndex) 
        {
            gunahKecisiIndex = Random.Range(0, tumSupheliler.Count);
        }
        Supheli gunahKecisi = tumSupheliler[gunahKecisiIndex];

        // Konsola kopya (Test için)
        Debug.Log($"KATİL: {katilKisi.isim} (İpucu: {katilKisi.ipucuTanimi})");
        Debug.Log($"İFTİRA KURBANI: {gunahKecisi.isim} (İpucu: {gunahKecisi.ipucuTanimi})");

        // 3. Herkese Rol Dağıt
        for (int i = 0; i < tumSupheliler.Count; i++)
        {
            Supheli suankiKisi = tumSupheliler[i];
            
            if (suankiKisi.npcScripti == null) continue;

            // --- İSİM GÖNDERME KISMI ---
            // Bu satırın çalışması için NpcDialogue scriptinde "public string npcAdi;" olmalı!
            suankiKisi.npcScripti.npcAdi = suankiKisi.isim; 
            // ---------------------------

            // --- KATİLSE ---
            if (i == katilIndex)
            {
                suankiKisi.npcScripti.buKisiKatilMi = true; 

                // Yazı tura: Savunma mı, İftira mı?
                if (Random.value < katilinIftiraIhtimali)
                {
                    // İFTİRA (Masumun özelliğini söyler)
                    string sablon = katilIftiralari[Random.Range(0, katilIftiralari.Length)];
                    suankiKisi.npcScripti.npcMesaji = string.Format(sablon, gunahKecisi.ipucuTanimi);
                }
                else
                {
                    // SAVUNMA
                    suankiKisi.npcScripti.npcMesaji = katilSavunmalari[Random.Range(0, katilSavunmalari.Length)];
                }
            }
            // --- MASUMSA ---
            else
            {
                suankiKisi.npcScripti.buKisiKatilMi = false;

                // Zar at: Gördü mü, Görmedi mi?
                if (Random.value < gormemeIhtimali)
                {
                    // GÖRMEDİ (Boş yapıyor)
                    suankiKisi.npcScripti.npcMesaji = bosSablonlar[Random.Range(0, bosSablonlar.Length)];
                }
                else
                {
                    // GÖRDÜ (Katilin özelliğini söyler)
                    string sablon = masumSablonlari[Random.Range(0, masumSablonlari.Length)];
                    suankiKisi.npcScripti.npcMesaji = string.Format(sablon, katilKisi.ipucuTanimi);
                }
            }
        }
    }
}