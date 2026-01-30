# 🔗 ANIMATOR TRANSITIONS KURULUM REHBERI

## ✅ Transitions Kontrol Listesi

Animator Controller'ında bu geçişlerin (transitions) var olması gerekiyor:

### 1️⃣ **Idle ↔ Walk**
- **Idle → Walk**
  - Condition: Speed > 0.1
  - Duration: 0.25 sn
  - Has Exit Time: ☐

- **Walk → Idle**
  - Condition: Speed ≤ 0.1
  - Duration: 0.25 sn
  - Has Exit Time: ☐

### 2️⃣ **Walk ↔ Run**
- **Walk → Run**
  - Condition: Speed > 0.6
  - Duration: 0.25 sn
  - Has Exit Time: ☐

- **Run → Walk**
  - Condition: Speed ≤ 0.6
  - Duration: 0.25 sn
  - Has Exit Time: ☐

### 3️⃣ **Any State → Jump**
- **Any State → Jump**
  - Condition: IsJumping (Trigger)
  - Duration: 0.1 sn
  - Has Exit Time: ☐

### 4️⃣ **Jump → Idle**
- **Jump → Idle**
  - Exit Time: 0.9 (animasyon %90'ı bittikten sonra)
  - Duration: 0.25 sn
  - Has Exit Time: ✓

---

## 🎮 UNITY EDITOR'DA MANUEL AYARLAMA

### Eğer transitions görmüyorsa, şu adımları izle:

#### ADIM 1: Animator Controller'ı Aç
```
1. Project → Assets/Animators/
2. DedektifAnimator.controller
3. Double-click → Animator Editor açılır
```

#### ADIM 2: Idle → Walk Transition Ekle
```
1. Idle state'ine sağ tıkla
2. "Make Transition" seç
3. Walk state'ine tıkla
4. Transition'a tıkla (çizgiye)
5. Inspector → Conditions:
   ├─ Click "+" 
   ├─ Speed > 0.1 seç
```

#### ADIM 3: Walk → Idle Transition Ekle
```
1. Walk state'ine sağ tıkla
2. "Make Transition" seç
3. Idle state'ine tıkla
4. Transition'a tıkla
5. Inspector → Conditions:
   ├─ Click "+"
   ├─ Speed < 0.1 seç
```

#### ADIM 4: Walk → Run Transition Ekle
```
1. Walk state'ine sağ tıkla
2. "Make Transition" seç
3. Run state'ine tıkla
4. Transition'a tıkla
5. Inspector → Conditions:
   ├─ Click "+"
   ├─ Speed > 0.6 seç
```

#### ADIM 5: Run → Walk Transition Ekle
```
1. Run state'ine sağ tıkla
2. "Make Transition" seç
3. Walk state'ine tıkla
4. Transition'a tıkla
5. Inspector → Conditions:
   ├─ Click "+"
   ├─ Speed < 0.6 seç
```

#### ADIM 6: Any State → Jump Transition Ekle
```
1. "Any State"'e sağ tıkla (sol altta)
2. "Make Transition" seç
3. Jump state'ine tıkla
4. Transition'a tıkla
5. Inspector → Conditions:
   ├─ Click "+"
   ├─ IsJumping (Trigger) seç
6. Has Exit Time: ☐ UNCHECKED
```

#### ADIM 7: Jump → Idle Transition Ekle
```
1. Jump state'ine sağ tıkla
2. "Make Transition" seç
3. Idle state'ine tıkla
4. Transition'a tıkla
5. Inspector:
   ├─ Has Exit Time: ✓ CHECKED
   ├─ Exit Time: 0.9
   ├─ Conditions: (hiç koşul yok)
```

---

## 🎯 Transition Ayarları (Inspector'da)

Tüm Transitions için:

```
Transition settings:
├─ Has Exit Time:          ☐ (Jump'dan gelen hariç)
├─ Transition Duration:    0.25 (Wait, Jump → Any State 0.1)
├─ Transition Offset:      0
├─ Interruption Source:    None
├─ Ordered Interrupt:      ☐
├─ Can Transition To Self: ☐
```

---

## ✨ Sonuç Görünüm

Animator Editor'da şöyle görünmeli:

```
            ↑ Speed > 0.1
    ┌───────┴────────┐
    │                │
    ▼ Speed ≤ 0.1    │
┌─────────┐      ┌─────────┐
│  IDLE   │      │  WALK   │
└─────────┘      └────┬────┘
    ▲                 │
    │                 │ Speed > 0.6
    │                 ▼
    │            ┌─────────┐
    │            │   RUN   │
    │            └─────────┘
    │                 │
    │                 │ Speed ≤ 0.6
    │                 ▼
    │            ┌─────────┐
    └────────────│  JUMP   │
IsJumping       └─────────┘
(Any State)     Exit Time: 0.9
```

---

## 🐛 Hata Giderme

| Problem | Çözüm |
|---------|-------|
| **Transitions görünmüyor** | Animator Editor'ı kapat ve aç |
| **Walk'dan Run'a gitmiyor** | Speed condition'ı kontrol et (> 0.6) |
| **Jump'a gidemiyor** | Any State transitions'i kontrol et |
| **Döngü oluşuyor** | Exit Time'ı kontrol et (Jump's'da 0.9) |

---

## ✅ TEST

Play tuşuna bas ve kontrol et:
- ☐ W tuşu → Idle'dan Walk'a geçiyor mu?
- ☐ W+Shift → Walk'dan Run'a geçiyor mu?
- ☐ Space → Jump animasyonu oynanıyor mu?
- ☐ Tuş bırak → Idle'a dönüyor mu?
