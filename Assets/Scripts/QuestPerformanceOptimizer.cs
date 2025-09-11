using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

/// <summary>
/// Optimizes Unity settings for Quest performance
/// </summary>
public class QuestPerformanceOptimizer : MonoBehaviour
{
    [Header("Performance Settings")]
    [SerializeField] private bool enableOptimizationsOnStart = true;
    [SerializeField] private int targetFrameRate = 72; // Quest 2 default, Quest 3 can go to 90/120
    
    void Start()
    {
        if (enableOptimizationsOnStart)
        {
            OptimizeForQuest();
        }
    }
    
    [ContextMenu("Optimize for Quest")]
    public void OptimizeForQuest()
    {
        Debug.Log("[QuestOptimizer] Applying Quest performance optimizations...");
        
        // Set target frame rate
        Application.targetFrameRate = targetFrameRate;
        
        // Disable VSync for VR (handled by XR runtime)
        QualitySettings.vSyncCount = 0;
        
        // Optimize quality settings
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.shadowDistance = 10f;
        QualitySettings.antiAliasing = 0; // Use MSAA in URP instead
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.realtimeReflectionProbes = false;
        QualitySettings.softParticles = false;
        QualitySettings.softVegetation = false;
        
        // Optimize LOD settings
        QualitySettings.lodBias = 0.4f;
        QualitySettings.maximumLODLevel = 1;
        
        // Optimize texture streaming
        QualitySettings.streamingMipmapsActive = true;
        QualitySettings.streamingMipmapsMemoryBudget = 256;
        
        // Optimize physics
        Physics.defaultSolverIterations = 4;
        Physics.defaultSolverVelocityIterations = 1;
        
        // Optimize audio
        var config = AudioSettings.GetConfiguration();
        config.sampleRate = 22050; // Lower sample rate for better performance
        config.speakerMode = AudioSpeakerMode.Stereo;
        AudioSettings.Reset(config);
        
        Debug.Log("[QuestOptimizer] Quest optimizations applied successfully!"); // Force recompilation
    }
    
    void Update()
    {
        // Monitor performance in debug builds
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Time.frameCount % 60 == 0) // Check every 60 frames
        {
            float fps = 1.0f / Time.unscaledDeltaTime;
            if (fps < targetFrameRate * 0.8f) // If FPS drops below 80% of target
            {
                Debug.LogWarning($"[QuestOptimizer] Performance warning: FPS = {fps:F1}, Target = {targetFrameRate}");
            }
        }
        #endif
    }
}