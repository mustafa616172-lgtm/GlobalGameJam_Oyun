# 🚀 Dedektif Animasyon - Hızlı Başlangıç Rehberi

## ⚡ 30 Saniyede Setup

### Adım 1: GameObject Hazırla
```
1. Hierarchy'de Dedektif GameObject'i seç
2. Inspector → Add Component → Rigidbody
3. Inspector → Add Component → CapsuleCollider
4. Rigidbody'de: Constraints = Freeze Rotation (X, Y, Z)
```

### Adım 2: Script'leri Ekle
```
Inspector → Add Component → CharacterMovement
Inspector → Add Component → DedektifSetup
```

### Adım 3: Animator Ata
```
DedektifAnimator.controller dosyasını
Animator komponentinin "Controller" alanına sürükle
```

### Adım 4: Ground Layer Ayarla
```
Inspector → CharacterMovement
Ground Layer → "Ground" seç (sahada mevcut olmalı)
```

---

## 🎮 Oyuncu Kontrolleri

| Tuş | Aksiyon |
|-----|---------|
| **W** | İleri |
| **S** | Geri |
| **A** | Sola dön |
| **D** | Sağa dön |
| **Shift** (W/A/S/D ile) | Koşma |
| **Space** | Zıplama |

---

## 📊 Animasyon Durumları

| State | Speed | Trigger |
|-------|-------|---------|
| **Idle** | 0.0 | Hareket yok |
| **Walk** | 0.5 | W/A/S/D |
| **Run** | 1.0 | W/A/S/D + Shift |
| **Jump** | 0.0-1.0 | Space |

---

## 🔧 Yapılandırma Dosyaları

### DedektifAnimator.controller
```
Animasyon Durumları: 4
Geçişler: 7
Parametreler: 3 (Speed, IsJumping, IsGrounded)
```

### CharacterMovement.cs
```
Yürüme Hızı: 3.0 m/s
Koşma Hızı: 6.0 m/s
Zıplama Gücü: 5.0
```

### DedektifSetup.cs
```
Collider Merkez: (0, 1, 0)
Collider Yarıçap: 0.4
Collider Yükseklik: 2.0
```

---

## 🐛 Hızlı Tanı

### Problem: Animasyon değişmiyor
**Çözüm:** 
1. Console'de Debug.Log("Speed: " + animator.GetFloat("Speed"));
2. Speed parametresi 0-1 aralığında mı kontrol et
3. Transitions koşullarını kontrol et

### Problem: Karakter hareket etmiyor
**Çözüm:**
1. Rigidbody'nin Frozen olup olmadığını kontrol et
2. Ground Layer ayarını kontrol et
3. CharacterMovement script'inin ekli olup olmadığını kontrol et

### Problem: Collider hatalı görünüyor
**Çözüm:**
1. CapsuleCollider'ı seç
2. Center: (0, 1, 0)
3. Radius: 0.4
4. Height: 2.0

---

## 💾 Dosya Konumları

```
Assets/Animators/
└── DedektifAnimator.controller

Assets/Scripts/
├── CharacterMovement.cs
└── DedektifSetup.cs

Assets/Kaarakterler/Dedektif/
├── anakarakter@Walking.fbx
├── anakarakter@Running.fbx
├── anakarakter@Jumping.fbx
└── texture files...
```

---

## 📞 API Referans

### DedektifSetup sınıfı
```csharp
// Speed parametresini ayarla (0-1)
dedektifSetup.SetAnimationSpeed(0.75f);

// Zıplama tetikle
dedektifSetup.TriggerJump();

// Yer teması güncelle
dedektifSetup.SetGrounded(true);

// Şu anki state'i al
string state = dedektifSetup.GetCurrentAnimationState();

// Debug bilgisi yazdır
dedektifSetup.DebugInfo();
```

---

## ✅ Kurulum Kontrol Listesi

- [ ] Rigidbody eklendi
- [ ] CapsuleCollider eklendi
- [ ] CharacterMovement script'i eklendi
- [ ] DedektifSetup script'i eklendi
- [ ] DedektifAnimator.controller atandı
- [ ] Ground Layer ayarlandı
- [ ] Console'de hata yok
- [ ] Karakter W/A/S/D ile hareket ediyor
- [ ] Shift ile koşma çalışıyor
- [ ] Space ile zıplama çalışıyor

---

## 🎯 Sonraki Adımlar

1. **Kamera Kontrolü:** mouselook.cs script'ini test et
2. **Ses Efektleri:** Adım sesleri ekle
3. **Animasyon Kinetik:** Speed transition hızlarını ayarla
4. **Karakter Seçenekleri:** Farklı karakterler için duplicate et

---

*Güncelleme Tarihi: 31 Ocak 2026*
*Durum: ✓ TAMAMLANMIŞ*
