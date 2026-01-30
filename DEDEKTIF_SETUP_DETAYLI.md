# 🎮 Dedektif Karakteri - Komple Setup Rehberi

## ✅ Tamamlanan Görevler

### 1. **Animasyon Dosyaları Kopyalandı** ✓
- ✅ `anakarakter@Walking.fbx` - Yürüme animasyonu
- ✅ `anakarakter@Running.fbx` - Koşma animasyonu  
- ✅ `anakarakter@Jumping.fbx` - Zıplama animasyonu
- ✅ Tüm `.meta` dosyaları

**Hedef Karakterler:**
- Adam1 karakteri ✓
- Kadın1 karakteri ✓

### 2. **C# Script: CharacterMovement.cs** ✓
📍 Lokasyon: `Assets/Scripts/CharacterMovement.cs`

**Features:**
- ✅ W tuşu → Yürüme animasyonu (Speed: 0.5)
- ✅ W + Left Shift → Koşma animasyonu (Speed: 1.0)
- ✅ Space → Zıplama animasyonu
- ✅ Tuş bırakılınca → Dursun (Idle)
- ✅ Ground detection (Raycast)
- ✅ Physics-based movement (Rigidbody)

**Ayarlanabilir Parametreler:**
```csharp
[SerializeField] private float walkSpeed = 3f;      // Yürüme hızı
[SerializeField] private float runSpeed = 6f;       // Koşma hızı
[SerializeField] private float jumpForce = 5f;      // Zıplama gücü
[SerializeField] private float groundDrag = 5f;     // Yer dragı
[SerializeField] private float airDrag = 0.5f;      // Hava dragı
[SerializeField] private float rayDistance = 0.5f;  // Ray uzaklığı
```

### 3. **Animator Controller: DedektifAnimator.controller** ✓
📍 Lokasyon: `Assets/Animators/DedektifAnimator.controller`

**Animation States:**
```
┌─────────────────────────────────────────────┐
│          Idle (Başlangıç)                   │
│     (Karakterin hareketsiz durumu)          │
└──────────┬──────────────────────┬───────────┘
           │ Speed > 0.1          │ Speed > 0.6
           ▼                      ▼
      ┌────────────┐         ┌────────────┐
      │   Walk     │────────▶│    Run     │
      │(Yürüme)    │Speed 0.6│  (Koşma)   │
      └────────────┘         └────────────┘
           │                       │
           └──────────┬────────────┘
                      │ Space Basıldığında
                      ▼
               ┌─────────────┐
               │    Jump     │
               │  (Zıplama)  │
               └──────┬──────┘
                      │ Animasyon Bittikten
                      │ Sonra Otomatik
                      ▼
                   Idle'a Dön
```

**Parameters:**
- `Speed` (Float): 0 = Idle, 0.5 = Walk, 1.0 = Run
- `IsJumping` (Trigger): Zıplama trigger'ı
- `IsGrounded` (Bool): Yer kontrol bayrağı

---

## 🔧 Unity Editor'de Kurulum Adımları

### ADIM 1: Dedektif Karakterini Scene'e Ekle

1. **Hierarchy Panel**'de sağ tıkla → 3D Object → Cube (Yer tutucu olarak)
2. **Project Panel**'den sürükle:
   - `Assets/Kaarakterler/Dedektif/822920de5c17b3ab8cecf473978e6343.obj`
   - Scene'ye bırak (veya Create Empty → isimlendirip modelini ekle)
3. Karaktere ad ver: **"Dedektif"**

### ADIM 2: Animator Component Ekle

1. Hierarchy'de Dedektif karakterini seç
2. Inspector → Add Component → Animator
3. **Controller** field'ine sürükle:
   - `Assets/Animators/DedektifAnimator.controller`

### ADIM 3: Rigidbody Component Ekle

1. Inspector → Add Component → Rigidbody
2. **Ayarları** yapılandır:
   ```
   Mass:                    1
   Drag:                    0
   Angular Drag:            0.05
   Use Gravity:             ✓ (checked)
   Is Kinematic:            ☐ (unchecked)
   Constraints:             Freeze Rotation X, Y, Z
   Collision Detection:     Continuous
   ```

### ADIM 4: Collider Ekle

1. Inspector → Add Component → Capsule Collider
   ```
   Center:      X=0, Y=0, Z=0
   Radius:      0.5
   Height:      2
   Direction:   Y-Axis
   ```

### ADIM 5: CharacterMovement Script Ekle

1. Inspector → Add Component → Character Movement
2. **Script Parametrelerini Ayarla:**
   ```
   Walk Speed:         3
   Run Speed:          6
   Jump Force:         5
   Ground Drag:        5
   Air Drag:           0.5
   Ray Distance:       0.5
   ```

### ADIM 6: Ground (Zemin) Oluştur

1. Hierarchy → Create 3D Object → Plane
2. Scale'i ayarla:
   ```
   Scale X: 50
   Scale Y: 1
   Scale Z: 50
   Position Y: -1 (karakterin altında)
   ```
3. **Add Component** → Box Collider (Is Trigger: unchecked)

---

## 🎮 Kontrol Şeması

```
┌─────────────────────────────────────────┐
│         DEDEKTIF KARAKTERI              │
│           Kontrol Şeması                │
├─────────────────────────────────────────┤
│                                         │
│  [W] TUŞU                               │
│  └─► Yürüme Animasyonu (Walk)          │
│      Speed Param: 0.5                   │
│                                         │
│  [W] + [LEFT SHIFT] TUŞLARI             │
│  └─► Koşma Animasyonu (Run)            │
│      Speed Param: 1.0                   │
│                                         │
│  [SPACE] TUŞU                           │
│  └─► Zıplama Animasyonu (Jump)         │
│      Trigger: IsJumping                 │
│                                         │
│  [TUŞLAR BIRAKIL]                       │
│  └─► Dursun Animasyonu (Idle)          │
│      Speed Param: 0                     │
│                                         │
└─────────────────────────────────────────┘
```

---

## 📊 Script Flow Diyagramı

```
Update()
│
├─ HandleInput()
│  ├─ W tuşu kontrolü
│  ├─ Shift tuşu kontrolü
│  ├─ Space tuşu kontrolü
│  └─ moveDirection, currentSpeed set et
│
├─ CheckGrounded()
│  └─ Raycast ile yer kontrolü
│
└─ UpdateAnimation()
   ├─ Speed parameter'ı hesapla
   ├─ IsGrounded parameter'ı set et
   └─ Animator'a gönder

FixedUpdate()
│
└─ Physics
   ├─ Velocity hesapla
   ├─ Gravity koruma
   └─ Drag ayarla (yer vs hava)
```

---

## 🐛 Troubleshooting (Sorun Çözme)

| Problem | Çözüm |
|---------|-------|
| **Karakter hareket etmiyor** | Rigidbody'nin Use Gravity ✓, Is Kinematic ☐ olduğunu kontrol et |
| **Animasyon değişmiyor** | Animator Controller'ın doğru atandığını kontrol et |
| **Zıplama çalışmıyor** | Ground Detection'ı kontrol et, Ray Distance'ı artır |
| **Karakter yapışıyor** | Drag değerlerini azalt, Collider'ları kontrol et |
| **Animasyon gecikmesi** | Animator Controller'da transition sürelerini kontrol et |

---

## 📁 Proje Yapısı

```
GlobalGameJam_Oyun/
├── Assets/
│   ├── Scripts/
│   │   └── CharacterMovement.cs .......................... ✓ Hazır
│   ├── Animators/
│   │   └── DedektifAnimator.controller .................. ✓ Hazır
│   ├── Kaarakterler/
│   │   ├── Adam1/
│   │   │   ├── anakarakter@Walking.fbx .................. ✓ Kopyalandı
│   │   │   ├── anakarakter@Running.fbx .................. ✓ Kopyalandı
│   │   │   └── anakarakter@Jumping.fbx .................. ✓ Kopyalandı
│   │   ├── Kadın1/
│   │   │   ├── anakarakter@Walking.fbx .................. ✓ Kopyalandı
│   │   │   ├── anakarakter@Running.fbx .................. ✓ Kopyalandı
│   │   │   └── anakarakter@Jumping.fbx .................. ✓ Kopyalandı
│   │   └── Dedektif/
│   │       ├── anakarakter@Walking.fbx .................. ✓ Orijinal
│   │       ├── anakarakter@Running.fbx .................. ✓ Orijinal
│   │       └── anakarakter@Jumping.fbx .................. ✓ Orijinal
│   └── Scenes/
│       └── SampleScene.unity
├── DEDEKTIF_SETUP.md ...................................... ✓ Bu dosya
├── setup_dedektif.bat
└── setup_dedektif.sh
```

---

## ✨ Bonus: Script Kustomizasyon

Script'i Inspector'dan ayarlamak kolay:

**Hızlı Koşu Modu:**
```
Walk Speed: 3 → 5
Run Speed: 6 → 10
```

**Daha Yüksek Zıplama:**
```
Jump Force: 5 → 8
```

**Daha Kararlı Hareket:**
```
Ground Drag: 5 → 10
Air Drag: 0.5 → 2
```

---

## 🎯 Sonuç

✅ **Dedektif karakteri için:**
- Tüm animasyon dosyaları kopyalandı
- CharacterMovement script oluşturuldu
- Animator Controller kuruldu
- Tüm gerekli kontroller entegre edildi

🎮 **Oyun hazır!** Artık character'ı kontrol edebilirsin!

---

**Son Güncelleme:** 30.01.2026 19:52
**Script Version:** 1.0
**Unity Versiyonu:** 6.0+
