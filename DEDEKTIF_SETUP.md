# Dedektif Karakteri Setup Talimatları

## ✅ Tamamlanan İşlemler:

1. **CharacterMovement.cs Script** - Oluşturuldu
   - W tuşu: Yürüme (0.5x hız)
   - W + Left Shift: Koşma (1.0x hız)
   - Space: Zıplama
   - Tuş bırakılınca: Dursun (Idle)

2. **DedektifAnimator.controller** - Oluşturuldu
   - Animator States: Idle, Walk, Run, Jump
   - Transitions: Otomatik geçişler kurulmuş
   - Parameters: Speed (float), IsJumping (trigger), IsGrounded (bool)

3. **Animasyon Dosyaları** - Kopyalandı
   - anakarakter@Walking.fbx (Yürüme)
   - anakarakter@Running.fbx (Koşma)
   - anakarakter@Jumping.fbx (Zıplama)

## 🔧 Unity Editor'de Yapılması Gerekenler:

### 1. Scene'e Dedektif Karakteri Ekle:
```
Assets/Kaarakterler/Dedektif/822920de5c17b3ab8cecf473978e6343.obj 
dosyasını Scene'e drag-drop ile ekle
```

### 2. Karaktere Components Ekle:
- **Animator**:
  - Controller: Assets/Animators/DedektifAnimator.controller
  
- **Rigidbody**:
  - Body Type: Dynamic
  - Mass: 1
  - Drag: 5
  - Angular Drag: 0.05
  - Gravity: Enabled
  - Constraints: Freeze Rotation (X, Y, Z)

- **Collider**:
  - Capsule Collider veya Box Collider ekle (karakterin etrafında)

### 3. Script Ekle:
- **CharacterMovement** script'ini karaktere attach et
- Inspector'de ayarları kontrol et:
  - Walk Speed: 3
  - Run Speed: 6
  - Jump Force: 5
  - Ground Drag: 5
  - Air Drag: 0.5
  - Ray Distance: 0.5

### 4. Plane (Yer) Ekle (varsa):
- Eğer scene'de bir ground olmadıysa, basit bir Plane ekle
- Plane'e MeshCollider ekle

## 🎮 Kontroller:
- **W** : Yürüme
- **W + Left Shift** : Koşma
- **Space** : Zıplama
- **Tuş Bırak** : Durdur (Idle)

## 📝 Notlar:
- Animator otomatik olarak animasyonları transition yapacak
- Karakterin hareket hızları Inspector'den ayarlanabilir
- Jump gücü ihtiyaca göre değiştirilebilir
