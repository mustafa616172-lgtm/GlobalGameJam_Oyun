using UnityEngine;
using System.Collections.Generic;

public class MysteryManager : MonoBehaviour
{
    [System.Serializable]
    public class Supheli
    {
        public string isim; // Karakteri tanıman için (Örn: Şapkalı)
        public NpcDialogue npcScripti; // Sahnedeki objeyi buraya sürükle
        [TextArea] public string ipucuTanimi; // BURAYI SEN YAZACAKSIN (Örn: "kırmızı bir atkısı")
    }

    [Header("Şüpheli Listesi")]
    public List<Supheli> tumSupheliler;

    // ARTIK LİSTELER KODUN İÇİNDE HAZIR!
    private string[] masumSablonlari = new string[] 
    {
        "Tam emin değilim ama sanki üzerinde {0} vardı.",
        "Olay yerinden kaçan kişinin {0} dikkatimi çekti.",
        "Yüzünü saklıyordu ama {0} olduğunu net gördüm.",
        "Çok korkunçtu! Tek hatırladığım {0} olduğu.",
        "Buralarda {0} olan birini gördün mü? O çok şüpheliydi.",
        "Yemin ederim suçlu ben değilim! Ama {0} olan birinden şüpheleniyorum.",
        "Göz ucuyla baktığımda {0} olduğunu fark ettim.",
        "Katili arıyorsan {0} olan kişiye dikkat et.",
        "Her şey çok hızlı gelişti... Yine de {0} aklımda kalmış.",
        "Şu {0} olan tip var ya, bence aradığın kişi o.",
        "İnanamıyorum, az önce {0} olan biri buradan koşarak geçti!",
        "Karanlıktı ama {0} parlıyordu sanki.",
        "Polis bey, kaçan kişinin kesinlikle {0} vardı."
    };

    private string[] katilYalanlari = new string[] 
    {
        "Ben bütün gün buradaydım memur bey, hiçbir şey görmedim.",
        "Benim olayla bir ilgim yok, sadece işimi yapıyorum.",
        "Neden bana öyle bakıyorsun? Ben masumum!",
        "Burada bir sürü insan var, neden benimle uğraşıyorsun?",
        "Hiçbir şey bilmiyorum, lütfen beni rahat bırak.",
        "Katil mi? Ne katili? Ben sadece hava almaya çıkmıştım."
    };

    void Start()
    {
        RolleriDagit();
    }

    public void RolleriDagit()
    {
        if (tumSupheliler.Count == 0) return;

        // 1. Rastgele Katil Seç
        int katilIndex = Random.Range(0, tumSupheliler.Count);
        Supheli katilKisi = tumSupheliler[katilIndex];

        Debug.Log("KATİL: " + katilKisi.isim + " | İPUCU: " + katilKisi.ipucuTanimi);

        // 2. Rolleri Dağıt
        for (int i = 0; i < tumSupheliler.Count; i++)
        {
            Supheli suankiKisi = tumSupheliler[i];

            if (suankiKisi.npcScripti != null)
            {
                if (i == katilIndex)
                {
                    // --- KATİLSE ---
                    string secilenYalan = katilYalanlari[Random.Range(0, katilYalanlari.Length)];
                    suankiKisi.npcScripti.npcMesaji = secilenYalan;
                }
                else
                {
                    // --- MASUMSA ---
                    string secilenSablon = masumSablonlari[Random.Range(0, masumSablonlari.Length)];
                    // {0} yerine katilin ipucunu koy
                    string olusturulanCumle = string.Format(secilenSablon, katilKisi.ipucuTanimi);
                    suankiKisi.npcScripti.npcMesaji = olusturulanCumle;
                }
            }
        }
    }
}