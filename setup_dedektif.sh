#!/bin/bash
# Dedektif Karakteri Script Yükleme Aracı
# Bu script, Dedektif karakterini Unity projesine yükler

echo "========================================="
echo "Dedektif Karakteri Kurulum Başlıyor"
echo "========================================="
echo ""

# Dosya kontrolü
echo "[1/3] Script dosyaları kontrol ediliyor..."
if [ -f "Assets/Scripts/CharacterMovement.cs" ]; then
    echo "✓ CharacterMovement.cs bulundu"
else
    echo "✗ CharacterMovement.cs bulunamadı!"
    exit 1
fi

if [ -f "Assets/Animators/DedektifAnimator.controller" ]; then
    echo "✓ DedektifAnimator.controller bulundu"
else
    echo "✗ DedektifAnimator.controller bulunamadı!"
    exit 1
fi

echo ""
echo "[2/3] Animasyon dosyaları kontrol ediliyor..."

ANIM_FILES=(
    "Assets/Kaarakterler/Dedektif/anakarakter@Walking.fbx"
    "Assets/Kaarakterler/Dedektif/anakarakter@Running.fbx"
    "Assets/Kaarakterler/Dedektif/anakarakter@Jumping.fbx"
)

for file in "${ANIM_FILES[@]}"; do
    if [ -f "$file" ]; then
        echo "✓ $(basename $file) bulundu"
    else
        echo "✗ $(basename $file) bulunamadı!"
    fi
done

echo ""
echo "[3/3] Kurulum talimatları"
echo "========================================="
echo ""
echo "Unity Editor'de aşağıdaki adımları izleyin:"
echo ""
echo "1. Scene'e Dedektif karakterini ekleyin:"
echo "   Assets/Kaarakterler/Dedektif/822920de5c17b3ab8cecf473978e6343.obj"
echo ""
echo "2. Karaktere Animator Component ekleyin:"
echo "   - Controller: Assets/Animators/DedektifAnimator.controller"
echo ""
echo "3. Karaktere Rigidbody ekleyin:"
echo "   - Body Type: Dynamic"
echo "   - Constraints: Freeze Rotation"
echo ""
echo "4. Karaktere CharacterMovement Script ekleyin:"
echo "   - Walk Speed: 3"
echo "   - Run Speed: 6"
echo "   - Jump Force: 5"
echo ""
echo "5. Kontroller:"
echo "   - W: Yürüme"
echo "   - W + Left Shift: Koşma"
echo "   - Space: Zıplama"
echo ""
echo "========================================="
echo "Kurulum tamamlandı!"
echo ""
