# 🎮 DEDEKTIF KARAKTERİ - DETAYLI KURULUM REHBERİ

## 📋 Adım Adım Kurulum

### **ADIM 1: Dedektif Karakterini Scene'e Ekle**

#### 1.1 Zemin Oluştur (Plane)
```
1. Hierarchy panelinde sağ tıkla
2. 3D Object → Plane seç
3. İsim: "Ground"
4. Position: X=0, Y=-1, Z=0
5. Scale: X=50, Y=1, Z=50
6. Add Component → Box Collider
   (Is Trigger: ☐ unchecked)
```

#### 1.2 Dedektif Karakteri Ekle
```
1. Hierarchy → Empty Create
2. İsim: "Dedektif"
3. Position: X=0, Y=0, Z=0
4. Position: X=0, Y=0, Z=0 (sıfırla)
```

#### 1.3 Model Ekle
```
1. Project → Assets/Kaarakterler/Dedektif/
2. 822920de5c17b3ab8cecf473978e6343.obj dosyasını
3. Dedektif GameObject'ine sürükle (child olarak)
4. Scale ayarla: X=1, Y=1, Z=1
```

---

### **ADIM 2: Avatar Setup (Humanoid Rig)**

#### 2.1 OBJ Dosyasının Import Ayarlarını Düzenle
```
1. Project → Assets/Kaarakterler/Dedektif/
2. 822920de5c17b3ab8cecf473978e6343.obj seç
3. Inspector → Model tab
4. Animation Type: Humanoid
5. Avatar Definition: Create From This Model
6. Apply → seç

NOT: Eğer hata alırsan, Generic olarak devam edebilirsin
```

#### 2.2 Animator Avatar Assign
```
1. Dedektif GameObject'i seç
2. Inspector → Animator component
3. Avatar field'ine:
   - Assets/Kaarakterler/Dedektif/
   - 822920de5c17b3ab8cecf473978e6343@avatar.fbx
4. Drop et
```

---

### **ADIM 3: Animator Controller Assign**

```
1. Dedektif GameObject'i seç
2. Inspector → Animator component
3. Controller field'ine:
   Assets/Animators/DedektifAnimator.controller
4. Sürükle ve bırak (drag-drop)
```

✅ **Sonuç:** Controller "None (Runtime An...)" → "DedektifAnimator (Controller)"

---

### **ADIM 4: Rigidbody Component Ekle & Konfigüre**

```
1. Dedektif GameObject'i seç
2. Inspector → Add Component
3. "Rigidbody" yaz ve seç

Ayarlar:
┌──────────────────────────────────┐
│ Mass:                      1     │
│ Drag:                      0     │
│ Angular Drag:              0.05  │
│ Use Gravity:              ✓      │
│ Is Kinematic:             ☐      │
│ Collision Detection:      Continuous
│ Constraints:              ✓ Freeze Rotation X
│                           ✓ Freeze Rotation Y
│                           ✓ Freeze Rotation Z
└──────────────────────────────────┘
```

---

### **ADIM 5: Collider Ekleme**

#### 5.1 Capsule Collider (Vücut)
```
1. Dedektif GameObject'i seç
2. Add Component → Capsule Collider
3. Ayarlar:
   Center:    X=0, Y=1, Z=0
   Radius:    0.5
   Height:    2
   Direction: Y-Axis
   Is Trigger: ☐
```

#### 5.2 Box Collider (Ayaklar - Ground Detection)
```
1. Add Component → Box Collider
2. Ayarlar:
   Center:    X=0, Y=-0.8, Z=0
   Size:      X=1, Y=0.2, Z=1
   Is Trigger: ✓ (checked - bu önemli!)
   Tag:       "Ground"
```

---

### **ADIM 6: CharacterMovement Script Attach**

```
1. Dedektif GameObject'i seç
2. Inspector → Add Component
3. "CharacterMovement" yaz ve seç

Parametreler (Inspector'da ayarla):
┌──────────────────────────────────┐
│ Walk Speed:        3             │
│ Run Speed:         6             │
│ Jump Force:        5             │
│ Ground Drag:       5             │
│ Air Drag:          0.5           │
│ Ray Distance:      0.5           │
└──────────────────────────────────┘
```

---

### **ADIM 7: Animator Controller Konfigürasyonu Kontrol**

**DedektifAnimator.controller içinde:**

```
Parameters Tab'ında kontrol et:
┌─────────────────────────────┐
│ ✓ Speed (Float, default: 0) │
│ ✓ IsJumping (Trigger)       │
│ ✓ IsGrounded (Bool, true)   │
└─────────────────────────────┘

States Tab'ında kontrol et:
┌────────────────────────────────────┐
│ Idle State:                        │
│  ├─ Motion: None                  │
│  └─ Transitions: Walk, Jump       │
│                                    │
│ Walk State:                        │
│  ├─ Motion: Walking.fbx           │
│  └─ Transitions: Idle, Run, Jump  │
│                                    │
│ Run State:                         │
│  ├─ Motion: Running.fbx           │
│  └─ Transitions: Walk, Jump       │
│                                    │
│ Jump State:                        │
│  ├─ Motion: Jumping.fbx           │
│  └─ Transitions: Idle (exittime)  │
└────────────────────────────────────┘
```

---

### **ADIM 8: Animasyon Dosyalarını Assign**

**Animator Controller Editor'ı aç:**

```
1. Project → Assets/Animators/DedektifAnimator.controller
2. Double-click → Animator Editor açılır
3. Base Layer → States'i seç

Walk State için:
└─ Motion: anakarakter@Walking.fbx

Run State için:
└─ Motion: anakarakter@Running.fbx

Jump State için:
└─ Motion: anakarakter@Jumping.fbx
```

---

### **ADIM 9: Camera Setup**

```
1. Main Camera GameObject'i seç
2. Position:
   X=0, Y=1.5, Z=-5

3. (İsteğe bağlı) Camera'yı Dedektif'in child'ı yap:
   Dedektif → Camera'ı sürükle
   Position: X=0, Y=0.6, Z=0
```

---

### **ADIM 10: Final Kontroller**

Hepsi hazır mı?

```
☐ Dedektif GameObject oluşturuldu
☐ Model eklenmiş
☐ Animator component var
  ├─ Controller: DedektifAnimator.controller ✓
  └─ Avatar: Avatar dosyası ✓
☐ Rigidbody
  ├─ Use Gravity: ✓
  └─ Constraints: Freeze Rotation ✓
☐ Capsule Collider (Vücut)
☐ Box Collider (Ground Detection)
☐ CharacterMovement Script attach edilmiş
☐ Ground (Plane) GameObject oluşturulmuş
☐ Camera pozisyonu ayarlanmış
```

---

## 🎮 TEST ETME

**Play butonuna bas ve kontrol et:**

```
✓ W tuşu    → Karakter yürüyor mü?
✓ W+Shift   → Karakter koşuyor mu?
✓ Space     → Karakter zıplayabiliyor mu?
✓ Yerden düşüyor mu? (Gravity)
✓ Hareket sınırlanıyor mu? (Constraints)
```

---

## 🐛 Sorun Çözme Tablosu

| Sorun | Çözüm |
|-------|-------|
| **Karakter görünmüyor** | Model GameObject'i kontrol et, position sıfırla |
| **Hareket etmiyor** | Rigidbody var mı? Is Kinematic ☐ mı? |
| **Animasyon değişmiyor** | Controller assign edildi mi? Parameters doğru mu? |
| **Yerden ayrılıyor** | Gravity ✓ mı? Rigidbody mass ayarla |
| **Zıplama sonsuz** | Ground detection, Ray Distance kontrol et |
| **Karakter dönüyor** | Freeze Rotation X,Y,Z ✓ mı? |
| **Hareketler tuhaf** | Script hata gösteriyor mu? Console kontrol et |

---

## 📐 Referans Değerleri

```csharp
// Script Varsayılan Değerleri
walkSpeed = 3f;
runSpeed = 6f;
jumpForce = 5f;
groundDrag = 5f;
airDrag = 0.5f;
rayDistance = 0.5f;

// Rigidbody
Mass: 1
Use Gravity: true
Constraints: Freeze Rotation (XYZ)

// Colliders
Capsule Height: 2, Radius: 0.5
Box Size: 1x0.2x1
```

---

## ✅ Hazır!

Tüm adımları tamamladıktan sonra:
1. Play butonu
2. W, Space, Shift tuşlarını dene
3. Animasyonları izle

🚀 **Dedektif karakteri artık tam kontrollü!**
