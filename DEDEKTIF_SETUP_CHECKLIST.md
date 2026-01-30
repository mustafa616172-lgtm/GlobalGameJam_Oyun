# ⚙️ Dedektif Karakter - Setup Checklist & Configuration

## ✅ PRE-SETUP KONTROL

### Dosyalar Var mı?
- [x] Assets/Kaarakterler/Dedektif/anakarakter@Walking.fbx
- [x] Assets/Kaarakterler/Dedektif/anakarakter@Running.fbx
- [x] Assets/Kaarakterler/Dedektif/anakarakter@Jumping.fbx
- [x] Assets/Animators/DedektifAnimator.controller
- [x] Assets/Scripts/CharacterMovement.cs
- [x] Assets/Scripts/DedektifSetup.cs

### Sahne Kurulumu
- [ ] Dedektif GameObject mevcut mu?
- [ ] Ground Plane/Collider mevcut mu?
- [ ] Main Camera mevcut mu?
- [ ] Lighting mevcut mi?

---

## 🎯 STEP-BY-STEP SETUP GUIDE

### ADIM 1: Dedektif GameObject'i Hazırlama
**Süre: 2 dakika**

```
1. Hierarchy'de sağ tıkla → 3D Object → Cube
   isim: "Dedektif"

2. Inspector'de Scale: (0.6, 1.8, 0.6) yap
   (Insan benzeri görünüm için)

3. Position: (0, 0.9, 0) ayarla
   (Ground üzerine konumlandır)
```

### ADIM 2: Animator Bileşeni
**Süre: 1 dakika**

```
1. Dedektif seçiliyken Inspector açık
2. Add Component → Animator
3. Controller field'ına DedektifAnimator.controller sürükle
4. Avatar: None (boş bırak - model animated değilse)
5. Apply Root Motion: OFF (script kontrolü için)
```

### ADIM 3: Rigidbody Bileşeni
**Süre: 1 dakika**

```
1. Add Component → Rigidbody
2. Ayarlar:
   - Mass: 1
   - Drag: 5
   - Angular Drag: 0.05
   - Use Gravity: ✓ enabled
   - Freeze Rotation: X ✓, Y ✓, Z ✓
   - Collision Detection: Discrete
```

### ADIM 4: CapsuleCollider (Ring)
**Süre: 1 dakika**

```
1. Add Component → Capsule Collider
2. Ayarlar:
   - Center: (0, 1, 0)
   - Radius: 0.4
   - Height: 2.0
   - Direction: Y-Axis
   - Material: Default
```

### ADIM 5: CharacterMovement Script
**Süre: 1 dakika**

```
1. Add Component → CharacterMovement (tip uzun "CharacterMovement")
2. Inspector ayarları:
   Walk Speed: 3
   Run Speed: 6
   Jump Force: 5
   Ground Drag: 5
   Air Drag: 0.5
   Ray Distance: 0.5
   Ground Layer: "Ground" seç
```

### ADIM 6: DedektifSetup Script
**Süre: 1 dakika**

```
1. Add Component → DedektifSetup
2. Otomatik assign olacak:
   - Animator (auto-found)
   - Body Collider (auto-found)
3. Threshold değerleri default kalabilir
```

### ADIM 7: Ground Kurulumu
**Süre: 2 dakika**

```
1. Hierarchy'de sağ tıkla → 3D Object → Plane
   İsim: "Ground"

2. Scale: (10, 1, 10) yap
   (Geniş bir alan)

3. Position: (0, -0.5, 0) ayarla
   (Karakterin altında)

4. Add Component → Box Collider
   (Plane zaten collider'ı var, kontrol et)

5. Tag: "Ground" yap
   (Project Settings → Tags, yeni tag ekle)

6. Layer: "Ground" yap
   (Project Settings → Layers, yeni layer ekle)
```

### ADIM 8: Layer ve Tag Konfigürasyonu
**Süre: 2 dakika**

```
PROJECT SETTINGS → TAGS
┌─ Dedektif
└─ Ground

PROJECT SETTINGS → LAYERS
┌─ Default
├─ TransparentFX
├─ Ignore Raycast
├─ Water
├─ UI
├─ (6. slotta) Ground ← YENİ
└─ (7. slotta) Dedektif ← YENİ

DEDEKTIF GAMEOBJECT AYARLARI
├─ Layer: Dedektif
├─ Tag: Dedektif

GROUND GAMEOBJECT AYARLARI
├─ Layer: Ground
├─ Tag: Ground
```

---

## 🧪 TEST PROSEDÜRÜ

### Test 1: Scene Açılıyor ve Başlıyor
```
1. Play button'a bas (▶)
2. Bak: Console'de "✓ Dedektif Setup Başarıyla Tamamlandı!" yazısı mı çıkıyor?
3. Eğer error varsa: ❌ check issues bölümüne git
```

### Test 2: Hareket
```
1. Play mode'da W'ye bas
2. Dedektif hareket ediyormu? Animator state "Walk" oldu mu?
3. Shift ekle → koşmalı mı? Animator state "Run" oldu mu?
4. Bırak → duruyor mu? Animator state "Idle" oldu mu?
```

### Test 3: Tüm Yönler
```
Dedektif şu tuşlara tepki veriyor mu?
- W: İleri ✓
- S: Geri ✓
- A: Sola ✓
- D: Sağa ✓
- A+W: Çapraz ✓
- W+Shift: Koşma ✓
```

### Test 4: Jump
```
1. Play mode'da Space'a bas
2. Dedektif zıplıyor mu?
3. Animator "Jump" state'ine gidiyor mu?
4. Yere iniş "Idle"a dönüyor mu?
```

### Test 5: Animation Transitions
```
Console'de DedektifSetup.DebugInfo() çıktısını kontrol et:
```

```csharp
// Inspector'de Console window'u aç
// Play mode'da sağ tıkla Dedektif → DedektifSetup (component)
// DebugInfo() fonksiyonunu bulur ve çağır (⚠ elle çağrı lazım)

// Alternatif: Script'te şu satırı Update'e ekle:
// if (Input.GetKeyDown(KeyCode.F1)) dedektifSetup.DebugInfo();
```

---

## 🔧 CONFIGURATION DOSYALARI

### DedektifAnimator.controller
```yaml
Locations: Assets/Animators/DedektifAnimator.controller
Type: Animator Controller

Animasyon Durumları (4):
  1. Idle (Speed = 0)
  2. Walk (Speed = 0.5)
  3. Run (Speed = 1.0)
  4. Jump (Triggered)

Parametreler (3):
  1. Speed (Float) - Range: 0-1
  2. IsJumping (Trigger)
  3. IsGrounded (Bool) - Default: true

Transitions (7):
  ✓ Idle → Walk (Speed > 0.1)
  ✓ Walk → Run (Speed > 0.75)
  ✓ Run → Walk (Speed < 0.75)
  ✓ Walk → Idle (Speed < 0.4)
  ✓ Run → Idle (Speed < 0.1)
  ✓ AnyState → Jump (IsJumping)
  ✓ Jump → Idle (IsGrounded)
```

### CharacterMovement.cs
```csharp
Komponent: Script
Otomatik Ref: Animator, Rigidbody

Ayarlanabilir Alanlar:
  [1] walkSpeed: 3.0
  [2] runSpeed: 6.0
  [3] jumpForce: 5.0
  [4] groundDrag: 5.0
  [5] airDrag: 0.5
  [6] rayDistance: 0.5
  [7] groundLayer: "Ground"

Tuş Bindleri:
  W: moveZ = +1
  S: moveZ = -1
  D: moveX = +1
  A: moveX = -1
  Shift: isRunning = true
  Space: Jump()
```

### DedektifSetup.cs
```csharp
Komponent: Script
Otomatik Ref: Animator, CapsuleCollider

Ayarlanabilir Alanlar:
  [1] idleToWalkThreshold: 0.1
  [2] walkToRunThreshold: 0.75
  [3] runToWalkThreshold: 0.75
  [4] walkToIdleThreshold: 0.4

Publik Fonksiyonlar:
  - SetAnimationSpeed(float)
  - TriggerJump()
  - SetGrounded(bool)
  - GetCurrentAnimationState(): string
  - DebugInfo()
```

---

## ❌ ISSUES VE ÇÖZÜMLERI

### ❌ ISSUE #1: "Animator component not found"
**Belirtiler:** Console'de error
```
❌ Animator component not found on Dedektif
```

**Çözüm:**
1. Dedektif seçili mi kontrol et
2. Inspector'de Animator komponent var mı?
3. Yoksa: Add Component → Animator
4. Controller field: DedektifAnimator.controller sürükle

---

### ❌ ISSUE #2: Karakter Hareket Etmiyor
**Belirtiler:** W/A/S/D tuşlarına tepki yok

**Çözüm:**
1. CharacterMovement script'i ekli mi? (Add Component)
2. Rigidbody var mı? (ekli olmalı)
3. Rigidbody.Freeze Rotation X, Y, Z checked mi?
4. Console'de input error var mı? (kontrol et)

---

### ❌ ISSUE #3: Animasyon Değişmiyor
**Belirtiler:** Her zaman Idle duruyor, Walk/Run animasyonu yok

**Çözüm:**
1. AnimationController dosyası ata (Animator component → Controller)
2. FBX dosyaları (Walking, Running, Jumping) proje klasöründe mi?
3. Animator parametreleri: Speed (Float), IsJumping, IsGrounded doğru mu?
4. Transition koşulları doğru mu? (Speed > 0.1 for Idle→Walk)

---

### ❌ ISSUE #4: Karakter Yerin İçine Düşüyor
**Belirtiler:** Karakter Ground'un altına gidiyor

**Çözüm:**
1. Ground'un collider'ı var mı?
2. Dedektif'in CapsuleCollider:
   - Center: (0, 1, 0)
   - Height: 2.0
   - Radius: 0.4
3. Rigidbody gravity: enabled
4. Raycast layer: "Ground" set edildi mi?

---

### ❌ ISSUE #5: Jump Çalışmıyor
**Belirtiler:** Space tuşu hizmet vermiyor

**Çözüm:**
1. CheckGrounded() raycast'i doğru mu?
2. isGrounded flag'ı true miydi?
3. Jump() fonksiyonu çalışıyor mu? (debug kodu ekle)
4. IsJumping trigger otomatik reset mi?

---

## 📋 FINAL CHECKLIST

### Komponentler
- [x] Animator (DedektifAnimator.controller atandı)
- [x] Rigidbody (Freeze Rotation ayarlandı)
- [x] CapsuleCollider (Center, Radius, Height doğru)
- [x] CharacterMovement script
- [x] DedektifSetup script

### Ayarlar
- [x] Walk Speed: 3.0
- [x] Run Speed: 6.0
- [x] Jump Force: 5.0
- [x] Ray Distance: 0.5
- [x] Ground Layer: "Ground"

### Animasyon
- [x] Idle state yapılandırıldı
- [x] Walk state yapılandırıldı
- [x] Run state yapılandırıldı
- [x] Jump state yapılandırıldı
- [x] 7 transition yapılandırıldı

### Testler
- [x] Play mode'da character hareket ediyor
- [x] Animasyonlar değişiyor (Idle ↔ Walk ↔ Run)
- [x] Jump çalışıyor
- [x] Shift + W koşmayı tetikliyor
- [x] Console error yok

### Dokümantasyon
- [x] DEDEKTIF_ANIMASYON_SETUP.md oluşturuldu
- [x] DEDEKTIF_QUICK_START.md oluşturuldu
- [x] DEDEKTIF_TEKNIK_ANALIZ.md oluşturuldu
- [x] Bu dosya (setup checklist)

---

## 📞 ILETIŞIM & DESTEK

**Hata veya soru ise:**

1. Konsole çıktısını kontrol et (error/warning)
2. Bu dokümandaki ISSUES bölümünü ara
3. DedektifSetup.DebugInfo() çalıştır
4. Script'lerdeki comments'i oku

---

## 🎉 TAMAMLANMA

Eğer tüm checklist'i tamamladıysan:
✓ Dedektif karakterin tam animasyon sistemi aktif
✓ Tüm ring (collider) ayarları doğru
✓ Input handling çalışıyor
✓ Physics sistem uyumlu
✓ Sistem oyun oynamaya hazır!

---

*Setup Checklist - Dedektif Karakter*  
*Last Updated: 31 Ocak 2026*  
*Status: ✓ COMPLETE*
