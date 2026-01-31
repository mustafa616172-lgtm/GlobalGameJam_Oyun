using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class DedektifAnimatorSetup
{
    [MenuItem("Tools/Setup Dedektif Animator")]
    public static void SetupAnimator()
    {
        string controllerPath = "Assets/Character/Dedektif/DedektifAnimator.controller";
        
        // Animator Controller dosyası varsa sil
        if (File.Exists(controllerPath))
        {
            AssetDatabase.DeleteAsset(controllerPath);
        }
        
        // Yeni Animator Controller oluştur
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        
        // FBX dosyalarından AnimationClip'leri yükle
        AnimationClip walkingClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Walking.fbx");
        AnimationClip runningClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Running.fbx");
        AnimationClip jumpingClip = LoadAnimationClip("Assets/Character/Dedektif/anakarakter@Jumping.fbx");
        
        // Parametreleri ekle
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("isJumping", AnimatorControllerParameterType.Bool);
        
        // Base layer'ı al
        AnimatorControllerLayer baseLayer = controller.layers[0];
        AnimatorStateMachine stateMachine = baseLayer.stateMachine;
        
        // Idle state (empty state)
        AnimatorState idleState = stateMachine.AddState("Idle", new Vector3(200, 0, 0));
        stateMachine.defaultState = idleState;
        
        // Walking state
        AnimatorState walkingState = stateMachine.AddState("Walking", new Vector3(200, 100, 0));
        if (walkingClip != null)
        {
            walkingState.motion = walkingClip;
        }
        
        // Running state
        AnimatorState runningState = stateMachine.AddState("Running", new Vector3(450, 100, 0));
        if (runningClip != null)
        {
            runningState.motion = runningClip;
        }
        
        // Jumping state
        AnimatorState jumpingState = stateMachine.AddState("Jumping", new Vector3(450, 200, 0));
        if (jumpingClip != null)
        {
            jumpingState.motion = jumpingClip;
        }
        
        // Transition'ları oluştur
        // Idle -> Walking (Speed > 0.1)
        AnimatorStateTransition idleToWalking = idleState.AddTransition(walkingState);
        idleToWalking.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        idleToWalking.duration = 0.25f;
        
        // Walking -> Idle (Speed < 0.1)
        AnimatorStateTransition walkingToIdle = walkingState.AddTransition(idleState);
        walkingToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        walkingToIdle.duration = 0.25f;
        
        // Walking -> Running (Speed > 5)
        AnimatorStateTransition walkingToRunning = walkingState.AddTransition(runningState);
        walkingToRunning.AddCondition(AnimatorConditionMode.Greater, 5f, "Speed");
        walkingToRunning.duration = 0.2f;
        
        // Running -> Walking (Speed < 5)
        AnimatorStateTransition runningToWalking = runningState.AddTransition(walkingState);
        runningToWalking.AddCondition(AnimatorConditionMode.Less, 5f, "Speed");
        runningToWalking.duration = 0.2f;
        
        // Any State -> Jumping (isJumping == true)
        AnimatorStateTransition anyToJumping = stateMachine.AddAnyStateTransition(jumpingState);
        anyToJumping.AddCondition(AnimatorConditionMode.If, 0, "isJumping");
        anyToJumping.duration = 0.1f;
        anyToJumping.canTransitionToSelf = false;
        
        // Jumping -> Idle (exit time)
        AnimatorStateTransition jumpingToIdle = jumpingState.AddExitTransition();
        jumpingToIdle.hasExitTime = true;
        jumpingToIdle.exitTime = 0.8f;
        jumpingToIdle.duration = 0.2f;
        
        // Controller'ı Dedektif karakterine ata
        GameObject dedektif = GameObject.Find("Dedektif");
        if (dedektif != null)
        {
            Animator animator = dedektif.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = controller;
                Debug.Log("✅ Dedektif Animator Controller başarıyla oluşturuldu ve atandı!");
            }
            else
            {
                Debug.LogError("❌ Dedektif'te Animator component bulunamadı!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Sahne'de Dedektif GameObject bulunamadı. Controller oluşturuldu ama otomatik atanamadı.");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"✅ Animator Controller oluşturuldu: {controllerPath}");
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
        Debug.LogWarning($"⚠️ AnimationClip bulunamadı: {fbxPath}");
        return null;
    }
}
