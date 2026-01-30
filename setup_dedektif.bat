@echo off
REM Dedektif Karakteri Kurulum Aracı - Windows

echo =========================================
echo Dedektif Karakteri Kurulum Baslaniyor
echo =========================================
echo.

REM Dosya kontrolü
echo [1/3] Script dosyalari kontrol ediliyor...

if exist "Assets\Scripts\CharacterMovement.cs" (
    echo [OK] CharacterMovement.cs bulundu
) else (
    echo [HATA] CharacterMovement.cs bulunamadi!
    pause
    exit /b 1
)

if exist "Assets\Animators\DedektifAnimator.controller" (
    echo [OK] DedektifAnimator.controller bulundu
) else (
    echo [HATA] DedektifAnimator.controller bulunamadi!
    pause
    exit /b 1
)

echo.
echo [2/3] Animasyon dosyalari kontrol ediliyor...

if exist "Assets\Kaarakterler\Dedektif\anakarakter@Walking.fbx" (
    echo [OK] anakarakter@Walking.fbx bulundu
)

if exist "Assets\Kaarakterler\Dedektif\anakarakter@Running.fbx" (
    echo [OK] anakarakter@Running.fbx bulundu
)

if exist "Assets\Kaarakterler\Dedektif\anakarakter@Jumping.fbx" (
    echo [OK] anakarakter@Jumping.fbx bulundu
)

echo.
echo [3/3] Kurulum talimlari
echo =========================================
echo.
echo Unity Editor'de asagidaki adimlari izleyin:
echo.
echo 1. Scene'e Dedektif karakterini ekleyin:
echo    Assets/Kaarakterler/Dedektif/822920de5c17b3ab8cecf473978e6343.obj
echo.
echo 2. Karaktere Animator Component ekleyin:
echo    - Controller: Assets/Animators/DedektifAnimator.controller
echo.
echo 3. Karaktere Rigidbody ekleyin:
echo    - Body Type: Dynamic
echo    - Constraints: Freeze Rotation
echo.
echo 4. Karaktere CharacterMovement Script ekleyin:
echo    - Walk Speed: 3
echo    - Run Speed: 6
echo    - Jump Force: 5
echo.
echo 5. Kontroller:
echo    - W: Yurume
echo    - W + Left Shift: Kosma
echo    - Space: Ziplama
echo.
echo =========================================
echo Kurulum tamamlandi!
echo.
pause
