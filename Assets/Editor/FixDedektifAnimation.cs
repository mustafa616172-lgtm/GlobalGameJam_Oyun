using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class FixDedektifAnimation
{
    [MenuItem("Tools/Fix Dedektif Animation [COMPLETE FIX]")]
    public static void CompleteAnimationFix()
    {
        Debug.Log("🔧 Dedektif Animasyon Tamiri Başlatılıyor...");
        
        // 1. FBX modelini sahneye ekle veya mevcut Dedektif'i değiştir
        GameObject dedektif = GameObject.Find("Dedektif");
        if (dedektif == null)
        {
            Debug.LogError("❌ Dedektif GameObject bulunamadı!");
            return;
        }
        
        // 2. Walking FBX'inden model prefab'ını yükle
        string fbxPath = "Assets/Character/Dedektif/anakarakter@Walking.fbx";
        GameObject fbxModel = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
        
        if (fbxModel == null)
        {
            Debug.LogError($"❌ FBX model yüklenemedi: {fbxPath}");
            return;
        }
        
        // 3. FBX'ten mesh ve rig al
        Transform dedektifTransform = dedektif.transform;
        Vector3 originalPosition = dedektifTransform.position;
        Quaternion originalRotation = dedektifTransform.rotation;
        Vector3 originalScale = dedektifTransform.localScale;
        Transform originalParent = dedektifTransform.parent;
        
        // 4. Mevcut Animator component'i koru
        Animator existingAnimator = dedektif.GetComponent<Animator>();
        RuntimeAnimatorController existingController = existingAnimator != null ? existingAnimator.runtimeAnimatorController : null;
        
        // 5. MeshFilter ve diğer component'leri güncelle
        MeshFilter meshFilter = dedektif.GetComponent<MeshFilter>();
        SkinnedMeshRenderer[] fbxSkinnedMeshes = fbxModel.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        // Eğer FBX'te SkinnedMeshRenderer varsa (rigged model)
        if (fbxSkinnedMeshes.Length > 0)
        {
            Debug.Log("✅ Rigged FBX modeli bulundu. SkinnedMeshRenderer kurulumu yapılıyor...");
            
            // Mevcut MeshFilter + MeshRenderer'ı kaldır
            MeshRenderer meshRenderer = dedektif.GetComponent<MeshRenderer>();
            if (meshFilter != null) Object.DestroyImmediate(meshFilter);
            if (meshRenderer != null) Object.DestroyImmediate(meshRenderer);
            
            // FBX'in skeleton yapısını kopyala
            GameObject instantiatedFBX = Object.Instantiate(fbxModel);
            instantiatedFBX.name = "_TempFBXInstance";
            
            // Skeleton ve mesh'i Dedektif'e aktar
            SkinnedMeshRenderer targetSMR = dedektif.GetComponent<SkinnedMeshRenderer>();
            if (targetSMR == null)
            {
                targetSMR = dedektif.AddComponent<SkinnedMeshRenderer>();
            }
            
            SkinnedMeshRenderer sourceSMR = fbxSkinnedMeshes[0];
            targetSMR.sharedMesh = sourceSMR.sharedMesh;
            targetSMR.materials = sourceSMR.sharedMaterials;
            
            // Bones yapısını kopyala
            Transform[] fbxBones = new Transform[sourceSMR.bones.Length];
            for (int i = 0; i < sourceSMR.bones.Length; i++)
            {
                Transform originalBone = sourceSMR.bones[i];
                
                // Dedektif altında aynı isimde bone var mı bak
                Transform existingBone = FindChildRecursive(dedektifTransform, originalBone.name);
                if (existingBone == null)
                {
                    // Bone yoksa skeleton yapısını kopyala
                    GameObject boneObject = new GameObject(originalBone.name);
                    boneObject.transform.SetParent(dedektifTransform);
                    boneObject.transform.localPosition = originalBone.localPosition;
                    boneObject.transform.localRotation = originalBone.localRotation;
                    boneObject.transform.localScale = originalBone.localScale;
                    existingBone = boneObject.transform;
                }
                
                fbxBones[i] = existingBone;
            }
            
            targetSMR.bones = fbxBones;
            targetSMR.rootBone = fbxBones.Length > 0 ? fbxBones[0] : null;
            
            // Geçici FBX instance'ını sil
            Object.DestroyImmediate(instantiatedFBX);
        }
        
        // 6. Animator'e Avatar ata
        if (existingAnimator == null)
        {
            existingAnimator = dedektif.AddComponent<Animator>();
        }
        
        // FBX'in Avatar'ını al
        Avatar fbxAvatar = AssetDatabase.LoadAssetAtPath<Avatar>(fbxPath);
        if (fbxAvatar != null)
        {
            existingAnimator.avatar = fbxAvatar;
            Debug.Log("✅ Avatar FBX'ten yüklendi ve atandı.");
        }
        else
        {
            Debug.LogWarning("⚠️ FBX'te Avatar bulunamadı. Generic avatar oluştur");
        }
        
        // 7. Controller'ı ata
        string controllerPath = "Assets/Character/Dedektif/DedektifAnimator.controller";
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        
        if (controller == null)
        {
            Debug.LogWarning("⚠️ DedektifAnimator.controller bulunamadı. Oluşturuluyor...");
            controller = CreateAnimatorController();
        }
        
        existingAnimator.runtimeAnimatorController = controller;
        existingAnimator.applyRootMotion = false;
        
        Debug.Log("✅ Dedektif Animasyon Tamiri Tamamlandı!");
        Debug.Log("   - Avatar ayarlandı");
        Debug.Log("   - Animator Controller atandı");
        Debug.Log("   🎮 Play Mode'a geçip test edin!");
    }
    
    private static Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
            
            Transform found = FindChildRecursive(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }
    
    private static AnimatorController CreateAnimatorController()
    {
        string controllerPath = "Assets/Character/Dedektif/DedektifAnimator.controller";
        
        if (File.Exists(controllerPath))
        {
            return AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
        }
        
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        
        // Animation clip'leri yükle
        AnimationClip walkingClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Walking.fbx");
        AnimationClip runningClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Running.fbx");
        AnimationClip jumpingClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Jumping.fbx");
        
        // Parametreleri ekle
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("isJumping", AnimatorControllerParameterType.Bool);
        
        // State machine
        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
        
        AnimatorState idleState = stateMachine.AddState("Idle", new Vector3(200, 0, 0));
        stateMachine.defaultState = idleState;
        
        AnimatorState walkingState = stateMachine.AddState("Walking", new Vector3(200, 100, 0));
        if (walkingClip != null) walkingState.motion = walkingClip;
        
        AnimatorState runningState = stateMachine.AddState("Running", new Vector3(450, 100, 0));
        if (runningClip != null) runningState.motion = runningClip;
        
        AnimatorState jumpingState = stateMachine.AddState("Jumping", new Vector3(450, 200, 0));
        if (jumpingClip != null) jumpingState.motion = jumpingClip;
        
        // Transitions
        var idleToWalking = idleState.AddTransition(walkingState);
        idleToWalking.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        idleToWalking.duration = 0.25f;
        
        var walkingToIdle = walkingState.AddTransition(idleState);
        walkingToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        walkingToIdle.duration = 0.25f;
        
        var walkingToRunning = walkingState.AddTransition(runningState);
        walkingToRunning.AddCondition(AnimatorConditionMode.Greater, 5f, "Speed");
        walkingToRunning.duration = 0.2f;
        
        var runningToWalking = runningState.AddTransition(walkingState);
        runningToWalking.AddCondition(AnimatorConditionMode.Less, 5f, "Speed");
        runningToWalking.duration = 0.2f;
        
        var anyToJumping = stateMachine.AddAnyStateTransition(jumpingState);
        anyToJumping.AddCondition(AnimatorConditionMode.If, 0, "isJumping");
        anyToJumping.duration = 0.1f;
        anyToJumping.canTransitionToSelf = false;
        
        var jumpingToIdle = jumpingState.AddExitTransition();
        jumpingToIdle.hasExitTime = true;
        jumpingToIdle.exitTime = 0.8f;
        
        AssetDatabase.SaveAssets();
        
        return controller;
    }
    
    private static AnimationClip LoadAnimationClip(string fbxPath)
    {
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        foreach (Object asset in assets)
        {
            if (asset is AnimationClip clip && !clip.name.Contains("__preview"))
            {
                return clip;
            }
        }
        return null;
    }
}
