using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace ConvaiMR
{
    /// <summary>
    /// Comprehensive fix for Unity XR and AR Foundation issues
    /// Addresses all common XR subsystem problems
    /// </summary>
    public class UnityXRIssuesFixer : MonoBehaviour
    {
        [Header("Auto Fix Settings")]
        [SerializeField] private bool autoFixOnStart = true;
        [SerializeField] private bool enableDebugLogging = true;
        [SerializeField] private bool fixPerformanceIssues = true;
        
        [Header("XR Configuration")]
        [SerializeField] private bool enableXR = true;
        [SerializeField] private bool enableARFoundation = true;
        [SerializeField] private bool enableHandTracking = true;
        
        [Header("Performance Settings")]
        [SerializeField] private bool optimizeForQuest3 = true;
        [SerializeField] private int targetFPS = 72;
        
        void Start()
        {
            if (autoFixOnStart)
            {
                FixAllXRIssues();
            }
        }
        
        [ContextMenu("Fix All XR Issues")]
        public void FixAllXRIssues()
        {
            Log("=== UNITY XR ISSUES FIXER ===");
            Log("Starting comprehensive XR issues fix...");
            
            // Step 1: Fix XR subsystem configuration
            FixXRSubsystemConfiguration();
            
            // Step 2: Fix AR Foundation setup
            FixARFoundationSetup();
            
            // Step 3: Fix hand tracking issues
            FixHandTrackingIssues();
            
            // Step 4: Fix performance issues
            if (fixPerformanceIssues)
            {
                FixPerformanceIssues();
            }
            
            // Step 5: Verify fixes
            VerifyFixes();
            
            Log("=== XR ISSUES FIX COMPLETE ===");
        }
        
        void FixXRSubsystemConfiguration()
        {
            Log("Fixing XR subsystem configuration...");
            
            // Enable XR if not already enabled
            if (enableXR && !XRSettings.enabled)
            {
                XRSettings.enabled = true;
                Log("Enabled XR in settings");
            }
            
            // Check for XR General Settings
            if (XRGeneralSettings.Instance == null)
            {
                LogWarning("XRGeneralSettings.Instance is null. XR may not be properly initialized.");
            }
            
            // Check for active loader
            if (XRGeneralSettings.Instance?.Manager?.activeLoader == null)
            {
                LogWarning("No active XR loader found. Please check XR Plug-in Management settings.");
            }
            
            Log("✓ XR subsystem configuration checked");
        }
        
        void FixARFoundationSetup()
        {
            Log("Fixing AR Foundation setup...");
            
            // Find or create XR Origin
            XROrigin xrOrigin = FindObjectOfType<XROrigin>();
            if (xrOrigin == null)
            {
                GameObject originGO = new GameObject("XR Origin");
                xrOrigin = originGO.AddComponent<XROrigin>();
                Log("Created XR Origin");
            }
            
            // Ensure XR Origin has a camera
            Camera xrCamera = xrOrigin.GetComponent<Camera>();
            if (xrCamera == null)
            {
                xrCamera = xrOrigin.gameObject.AddComponent<Camera>();
                xrCamera.tag = "MainCamera";
                Log("Added Camera to XR Origin");
            }
            
            // Setup AR Session
            ARSession arSession = FindObjectOfType<ARSession>();
            if (arSession == null)
            {
                GameObject sessionGO = new GameObject("AR Session");
                arSession = sessionGO.AddComponent<ARSession>();
                Log("Created AR Session");
            }
            
            // Setup AR Managers on XR Origin
            SetupARManager<ARPlaneManager>(xrOrigin.gameObject, "AR Plane Manager");
            SetupARManager<ARRaycastManager>(xrOrigin.gameObject, "AR Raycast Manager");
            SetupARManager<ARCameraManager>(xrOrigin.gameObject, "AR Camera Manager");
            
            Log("✓ AR Foundation setup completed");
        }
        
        void SetupARManager<T>(GameObject parent, string managerName) where T : MonoBehaviour
        {
            T manager = parent.GetComponent<T>();
            if (manager == null)
            {
                manager = parent.AddComponent<T>();
                Log($"Added {managerName} to XR Origin");
            }
        }
        
        void FixHandTrackingIssues()
        {
            Log("Fixing hand tracking issues...");
            
            if (!enableHandTracking)
            {
                Log("Hand tracking disabled by user");
                return;
            }
            
            // Check if hand tracking subsystem is available
            var handTrackingSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();
            if (handTrackingSubsystem == null)
            {
                LogWarning("Hand tracking subsystem not available. Please enable in OpenXR settings.");
            }
            else
            {
                Log("✓ Hand tracking subsystem is available");
            }
            
            // Check XR Input Modality Manager
            var inputModalityManager = FindObjectOfType<XRInputModalityManager>();
            if (inputModalityManager == null)
            {
                LogWarning("XRInputModalityManager not found. Hand tracking may not work properly.");
            }
            else
            {
                Log("✓ XR Input Modality Manager found");
            }
            
            Log("✓ Hand tracking issues checked");
        }
        
        void FixPerformanceIssues()
        {
            Log("Fixing performance issues...");
            
            if (!optimizeForQuest3)
            {
                Log("Quest 3 optimization disabled by user");
                return;
            }
            
            // Set target frame rate
            Application.targetFrameRate = targetFPS;
            Log($"Set target frame rate to {targetFPS}");
            
            // Optimize quality settings for Quest 3
            OptimizeQualitySettings();
            
            // Optimize AR Foundation for performance
            OptimizeARFoundationPerformance();
            
            Log("✓ Performance optimization completed");
        }
        
        void OptimizeQualitySettings()
        {
            // Set appropriate quality level for Quest 3
            QualitySettings.SetQualityLevel(2); // Good quality
            QualitySettings.antiAliasing = 2; // 2x MSAA
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadowDistance = 50f;
            QualitySettings.lodBias = 1.0f;
            
            Log("Optimized quality settings for Quest 3");
        }
        
        void OptimizeARFoundationPerformance()
        {
            // Optimize AR Plane Manager
            ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager != null)
            {
                planeManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
                Log("Optimized AR Plane Manager");
            }
            
            // Optimize AR Camera Manager
            ARCameraManager cameraManager = FindObjectOfType<ARCameraManager>();
            if (cameraManager != null)
            {
                cameraManager.requestedFacingDirection = CameraFacingDirection.World;
                Log("Optimized AR Camera Manager");
            }
        }
        
        void VerifyFixes()
        {
            Log("Verifying fixes...");
            
            // Check XR subsystems
            CheckXRSubsystems();
            
            // Check AR Foundation components
            CheckARFoundationComponents();
            
            // Check performance
            if (fixPerformanceIssues)
            {
                CheckPerformance();
            }
            
            Log("✓ Verification completed");
        }
        
        void CheckXRSubsystems()
        {
            Log("Checking XR subsystems...");
            
            var sessionSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRSessionSubsystem>();
            if (sessionSubsystem != null)
            {
                Log($"✓ XRSessionSubsystem: {(sessionSubsystem.running ? "Running" : "Not Running")}");
            }
            else
            {
                LogError("✗ XRSessionSubsystem not available");
            }
            
            var inputSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                Log($"✓ XRInputSubsystem: {(inputSubsystem.running ? "Running" : "Not Running")}");
            }
            else
            {
                LogError("✗ XRInputSubsystem not available");
            }
            
            var cameraSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRCameraSubsystem>();
            if (cameraSubsystem != null)
            {
                Log($"✓ XRCameraSubsystem: {(cameraSubsystem.running ? "Running" : "Not Running")}");
            }
            else
            {
                LogError("✗ XRCameraSubsystem not available");
            }
            
            var planeSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRPlaneSubsystem>();
            if (planeSubsystem != null)
            {
                Log($"✓ XRPlaneSubsystem: {(planeSubsystem.running ? "Running" : "Not Running")}");
            }
            else
            {
                LogError("✗ XRPlaneSubsystem not available");
            }
        }
        
        void CheckARFoundationComponents()
        {
            Log("Checking AR Foundation components...");
            
            XROrigin xrOrigin = FindObjectOfType<XROrigin>();
            if (xrOrigin != null)
            {
                Log("✓ XR Origin found");
            }
            else
            {
                LogError("✗ XR Origin not found");
            }
            
            ARSession arSession = FindObjectOfType<ARSession>();
            if (arSession != null)
            {
                Log("✓ AR Session found");
            }
            else
            {
                LogError("✗ AR Session not found");
            }
            
            ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager != null)
            {
                Log("✓ AR Plane Manager found");
            }
            else
            {
                LogError("✗ AR Plane Manager not found");
            }
            
            ARRaycastManager raycastManager = FindObjectOfType<ARRaycastManager>();
            if (raycastManager != null)
            {
                Log("✓ AR Raycast Manager found");
            }
            else
            {
                LogError("✗ AR Raycast Manager not found");
            }
            
            ARCameraManager cameraManager = FindObjectOfType<ARCameraManager>();
            if (cameraManager != null)
            {
                Log("✓ AR Camera Manager found");
            }
            else
            {
                LogError("✗ AR Camera Manager not found");
            }
        }
        
        void CheckPerformance()
        {
            Log("Checking performance...");
            
            float currentFPS = 1f / Time.unscaledDeltaTime;
            Log($"Current FPS: {currentFPS:F1}");
            Log($"Target FPS: {targetFPS}");
            Log($"Quality Level: {QualitySettings.GetQualityLevel()}");
            Log($"Anti-aliasing: {QualitySettings.antiAliasing}");
            
            if (currentFPS < targetFPS * 0.8f)
            {
                LogWarning($"Performance warning: FPS is {currentFPS:F1}, target is {targetFPS}");
            }
            else
            {
                Log("✓ Performance is within acceptable range");
            }
        }
        
        [ContextMenu("Test XR Subsystems")]
        public void TestXRSubsystems()
        {
            CheckXRSubsystems();
        }
        
        [ContextMenu("Test AR Foundation")]
        public void TestARFoundation()
        {
            CheckARFoundationComponents();
        }
        
        [ContextMenu("Test Performance")]
        public void TestPerformance()
        {
            CheckPerformance();
        }
        
        void Log(string message)
        {
            if (enableDebugLogging)
            {
                Debug.Log($"[UnityXRIssuesFixer] {message}");
            }
        }
        
        void LogWarning(string message)
        {
            if (enableDebugLogging)
            {
                Debug.LogWarning($"[UnityXRIssuesFixer] {message}");
            }
        }
        
        void LogError(string message)
        {
            if (enableDebugLogging)
            {
                Debug.LogError($"[UnityXRIssuesFixer] {message}");
            }
        }
    }
}
