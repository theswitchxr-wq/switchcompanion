using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

namespace ConvaiMR
{
    /// <summary>
    /// Optimizes performance specifically for Meta Quest 3
    /// Addresses FPS issues and improves overall performance
    /// </summary>
    public class Quest3PerformanceOptimizer : MonoBehaviour
    {
        [Header("Performance Settings")]
        [SerializeField] private float targetFPS = 72f;
        [SerializeField] private float warningThreshold = 60f;
        [SerializeField] private bool enableDynamicOptimization = true;
        [SerializeField] private bool enableDebugLogging = true;
        
        [Header("Optimization Levels")]
        [SerializeField] private PerformanceLevel currentLevel = PerformanceLevel.High;
        [SerializeField] private bool autoAdjustLevel = true;
        
        [Header("AR Foundation Optimization")]
        [SerializeField] private ARPlaneManager arPlaneManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARCameraManager arCameraManager;
        
        [Header("Rendering Optimization")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private int targetResolution = 1080;
        [SerializeField] private int maxTextureSize = 1024;
        
        // Performance monitoring
        private float[] fpsHistory = new float[60]; // 1 second of history at 60fps
        private int fpsIndex = 0;
        private float averageFPS = 0f;
        private float lastOptimizationTime = 0f;
        private const float optimizationInterval = 2f; // Check every 2 seconds
        
        public enum PerformanceLevel
        {
            Low,    // 45fps target, aggressive optimization
            Medium, // 60fps target, moderate optimization
            High,   // 72fps target, minimal optimization
            Ultra   // 90fps target, no optimization
        }
        
        void Start()
        {
            InitializeOptimization();
        }
        
        void Update()
        {
            if (enableDynamicOptimization)
            {
                MonitorPerformance();
                
                if (Time.time - lastOptimizationTime > optimizationInterval)
                {
                    OptimizePerformance();
                    lastOptimizationTime = Time.time;
                }
            }
        }
        
        void InitializeOptimization()
        {
            Log("Initializing Quest 3 performance optimization...");
            
            // Find components
            if (arPlaneManager == null)
                arPlaneManager = FindObjectOfType<ARPlaneManager>();
            
            if (arRaycastManager == null)
                arRaycastManager = FindObjectOfType<ARRaycastManager>();
            
            if (arCameraManager == null)
                arCameraManager = FindObjectOfType<ARCameraManager>();
            
            if (mainCamera == null)
                mainCamera = Camera.main;
            
            // Apply initial optimization
            ApplyPerformanceLevel(currentLevel);
            
            Log($"Performance optimization initialized at {currentLevel} level");
        }
        
        void MonitorPerformance()
        {
            // Calculate current FPS
            float currentFPS = 1f / Time.unscaledDeltaTime;
            fpsHistory[fpsIndex] = currentFPS;
            fpsIndex = (fpsIndex + 1) % fpsHistory.Length;
            
            // Calculate average FPS
            float totalFPS = 0f;
            for (int i = 0; i < fpsHistory.Length; i++)
            {
                totalFPS += fpsHistory[i];
            }
            averageFPS = totalFPS / fpsHistory.Length;
            
            // Log warnings if FPS is low
            if (averageFPS < warningThreshold)
            {
                LogWarning($"Low FPS detected: {averageFPS:F1} (Target: {targetFPS})");
            }
        }
        
        void OptimizePerformance()
        {
            if (!autoAdjustLevel) return;
            
            PerformanceLevel newLevel = DetermineOptimalLevel();
            
            if (newLevel != currentLevel)
            {
                Log($"Adjusting performance level from {currentLevel} to {newLevel}");
                currentLevel = newLevel;
                ApplyPerformanceLevel(currentLevel);
            }
        }
        
        PerformanceLevel DetermineOptimalLevel()
        {
            if (averageFPS >= 70f)
                return PerformanceLevel.Ultra;
            else if (averageFPS >= 60f)
                return PerformanceLevel.High;
            else if (averageFPS >= 45f)
                return PerformanceLevel.Medium;
            else
                return PerformanceLevel.Low;
        }
        
        void ApplyPerformanceLevel(PerformanceLevel level)
        {
            Log($"Applying performance level: {level}");
            
            switch (level)
            {
                case PerformanceLevel.Low:
                    ApplyLowPerformanceSettings();
                    break;
                case PerformanceLevel.Medium:
                    ApplyMediumPerformanceSettings();
                    break;
                case PerformanceLevel.High:
                    ApplyHighPerformanceSettings();
                    break;
                case PerformanceLevel.Ultra:
                    ApplyUltraPerformanceSettings();
                    break;
            }
        }
        
        void ApplyLowPerformanceSettings()
        {
            // Aggressive optimization for low-end performance
            targetFPS = 45f;
            
            // AR Foundation optimization
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
                // Reduce plane update frequency
                var planeManagerType = typeof(ARPlaneManager);
                var updateIntervalField = planeManagerType.GetField("m_PlaneUpdateInterval", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (updateIntervalField != null)
                {
                    updateIntervalField.SetValue(arPlaneManager, 1.0f); // Update every 1 second
                }
            }
            
            // Rendering optimization
            if (mainCamera != null)
            {
                mainCamera.allowMSAA = false;
                mainCamera.allowHDR = false;
            }
            
            // Quality settings
            QualitySettings.SetQualityLevel(0); // Fastest
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadowDistance = 20f;
            
            Log("Applied LOW performance settings");
        }
        
        void ApplyMediumPerformanceSettings()
        {
            // Moderate optimization
            targetFPS = 60f;
            
            // AR Foundation optimization
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
            }
            
            // Rendering optimization
            if (mainCamera != null)
            {
                mainCamera.allowMSAA = false;
                mainCamera.allowHDR = true;
            }
            
            // Quality settings
            QualitySettings.SetQualityLevel(1); // Simple
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowDistance = 30f;
            
            Log("Applied MEDIUM performance settings");
        }
        
        void ApplyHighPerformanceSettings()
        {
            // Minimal optimization for good performance
            targetFPS = 72f;
            
            // AR Foundation optimization
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
            }
            
            // Rendering optimization
            if (mainCamera != null)
            {
                mainCamera.allowMSAA = true;
                mainCamera.allowHDR = true;
            }
            
            // Quality settings
            QualitySettings.SetQualityLevel(2); // Good
            QualitySettings.antiAliasing = 2;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowDistance = 50f;
            
            Log("Applied HIGH performance settings");
        }
        
        void ApplyUltraPerformanceSettings()
        {
            // No optimization, maximum quality
            targetFPS = 90f;
            
            // AR Foundation optimization
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
            }
            
            // Rendering optimization
            if (mainCamera != null)
            {
                mainCamera.allowMSAA = true;
                mainCamera.allowHDR = true;
            }
            
            // Quality settings
            QualitySettings.SetQualityLevel(3); // Beautiful
            QualitySettings.antiAliasing = 4;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            QualitySettings.shadowResolution = ShadowResolution.High;
            QualitySettings.shadowDistance = 100f;
            
            Log("Applied ULTRA performance settings");
        }
        
        [ContextMenu("Force Low Performance")]
        public void ForceLowPerformance()
        {
            currentLevel = PerformanceLevel.Low;
            ApplyPerformanceLevel(currentLevel);
        }
        
        [ContextMenu("Force Medium Performance")]
        public void ForceMediumPerformance()
        {
            currentLevel = PerformanceLevel.Medium;
            ApplyPerformanceLevel(currentLevel);
        }
        
        [ContextMenu("Force High Performance")]
        public void ForceHighPerformance()
        {
            currentLevel = PerformanceLevel.High;
            ApplyPerformanceLevel(currentLevel);
        }
        
        [ContextMenu("Show Performance Stats")]
        public void ShowPerformanceStats()
        {
            Log("=== PERFORMANCE STATS ===");
            Log($"Current FPS: {1f / Time.unscaledDeltaTime:F1}");
            Log($"Average FPS: {averageFPS:F1}");
            Log($"Target FPS: {targetFPS}");
            Log($"Performance Level: {currentLevel}");
            Log($"Quality Level: {QualitySettings.GetQualityLevel()}");
            Log($"Anti-aliasing: {QualitySettings.antiAliasing}");
            Log($"Shadow Resolution: {QualitySettings.shadowResolution}");
            Log($"Shadow Distance: {QualitySettings.shadowDistance}");
            Log("========================");
        }
        
        [ContextMenu("Reset to Default")]
        public void ResetToDefault()
        {
            currentLevel = PerformanceLevel.High;
            ApplyPerformanceLevel(currentLevel);
            Log("Reset to default performance settings");
        }
        
        void Log(string message)
        {
            if (enableDebugLogging)
            {
                Debug.Log($"[Quest3PerformanceOptimizer] {message}");
            }
        }
        
        void LogWarning(string message)
        {
            if (enableDebugLogging)
            {
                Debug.LogWarning($"[Quest3PerformanceOptimizer] {message}");
            }
        }
    }
}
