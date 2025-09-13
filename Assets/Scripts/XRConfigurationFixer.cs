using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace ConvaiMR
{
    /// <summary>
    /// Fixes XR subsystem configuration issues and ensures proper AR Foundation setup
    /// </summary>
    public class XRConfigurationFixer : MonoBehaviour
    {
        [Header("XR Configuration")]
        [SerializeField] private bool autoFixOnStart = true;
        [SerializeField] private bool enableDebugLogging = true;
        
        [Header("AR Foundation Components")]
        [SerializeField] private ARSession arSession;
        [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private ARPlaneManager arPlaneManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARCameraManager arCameraManager;
        
        void Start()
        {
            if (autoFixOnStart)
            {
                FixXRConfiguration();
            }
        }
        
        [ContextMenu("Fix XR Configuration")]
        public void FixXRConfiguration()
        {
            Log("=== XR CONFIGURATION FIXER ===");
            Log("Starting XR subsystem configuration fix...");
            
            // Step 1: Check XR subsystem status
            CheckXRSubsystemStatus();
            
            // Step 2: Setup AR Foundation components
            SetupARFoundationComponents();
            
            // Step 3: Configure XR Origin
            ConfigureXROrigin();
            
            // Step 4: Setup AR Session
            SetupARSession();
            
            // Step 5: Configure AR Managers
            ConfigureARManagers();
            
            // Step 6: Verify configuration
            VerifyConfiguration();
            
            Log("=== XR CONFIGURATION COMPLETE ===");
        }
        
        void CheckXRSubsystemStatus()
        {
            Log("Checking XR subsystem status...");
            
            // Check if XR is enabled
            if (!XRSettings.enabled)
            {
                LogWarning("XR is not enabled! This will cause subsystem errors.");
                LogWarning("Please enable XR in Project Settings > XR Plug-in Management");
            }
            
            // Check for active subsystems
            var sessionSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRSessionSubsystem>();
            if (sessionSubsystem == null)
            {
                LogWarning("No active XRSessionSubsystem found");
            }
            else
            {
                Log("✓ XRSessionSubsystem is active");
            }
            
            var inputSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem == null)
            {
                LogWarning("No active XRInputSubsystem found");
            }
            else
            {
                Log("✓ XRInputSubsystem is active");
            }
        }
        
        void SetupARFoundationComponents()
        {
            Log("Setting up AR Foundation components...");
            
            // Find or create XR Origin
            if (xrOrigin == null)
            {
                xrOrigin = FindObjectOfType<XROrigin>();
                if (xrOrigin == null)
                {
                    GameObject originGO = new GameObject("XR Origin");
                    xrOrigin = originGO.AddComponent<XROrigin>();
                    Log("Created XR Origin");
                }
            }
            
            // Find or create AR Session
            if (arSession == null)
            {
                arSession = FindObjectOfType<ARSession>();
                if (arSession == null)
                {
                    GameObject sessionGO = new GameObject("AR Session");
                    arSession = sessionGO.AddComponent<ARSession>();
                    Log("Created AR Session");
                }
            }
            
            // Setup AR Managers on XR Origin
            if (xrOrigin != null)
            {
                // AR Plane Manager
                if (arPlaneManager == null)
                {
                    arPlaneManager = xrOrigin.GetComponent<ARPlaneManager>();
                    if (arPlaneManager == null)
                    {
                        arPlaneManager = xrOrigin.gameObject.AddComponent<ARPlaneManager>();
                        Log("Added ARPlaneManager to XR Origin");
                    }
                }
                
                // AR Raycast Manager
                if (arRaycastManager == null)
                {
                    arRaycastManager = xrOrigin.GetComponent<ARRaycastManager>();
                    if (arRaycastManager == null)
                    {
                        arRaycastManager = xrOrigin.gameObject.AddComponent<ARRaycastManager>();
                        Log("Added ARRaycastManager to XR Origin");
                    }
                }
                
                // AR Camera Manager
                if (arCameraManager == null)
                {
                    arCameraManager = xrOrigin.GetComponent<ARCameraManager>();
                    if (arCameraManager == null)
                    {
                        arCameraManager = xrOrigin.gameObject.AddComponent<ARCameraManager>();
                        Log("Added ARCameraManager to XR Origin");
                    }
                }
            }
        }
        
        void ConfigureXROrigin()
        {
            if (xrOrigin == null) return;
            
            Log("Configuring XR Origin...");
            
            // Ensure XR Origin has a camera
            Camera xrCamera = xrOrigin.GetComponent<Camera>();
            if (xrCamera == null)
            {
                xrCamera = xrOrigin.gameObject.AddComponent<Camera>();
                Log("Added Camera to XR Origin");
            }
            
            // Set camera as main camera
            if (xrCamera.tag != "MainCamera")
            {
                xrCamera.tag = "MainCamera";
                Log("Set XR Origin camera as MainCamera");
            }
            
            // Configure camera settings for AR
            xrCamera.clearFlags = CameraClearFlags.SolidColor;
            xrCamera.backgroundColor = Color.black;
            xrCamera.nearClipPlane = 0.01f;
            xrCamera.farClipPlane = 20f;
            
            Log("✓ XR Origin configured");
        }
        
        void SetupARSession()
        {
            if (arSession == null) return;
            
            Log("Setting up AR Session...");
            
            // Configure AR Session
            arSession.requestedTrackingMode = RequestedTrackingMode.Default;
            
            Log("✓ AR Session configured");
        }
        
        void ConfigureARManagers()
        {
            Log("Configuring AR Managers...");
            
            // Configure AR Plane Manager
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
                arPlaneManager.detectionMode = PlaneDetectionMode.HorizontalAndVertical;
                Log("✓ AR Plane Manager configured");
            }
            
            // Configure AR Camera Manager
            if (arCameraManager != null)
            {
                arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
                Log("✓ AR Camera Manager configured");
            }
            
            // Configure AR Raycast Manager
            if (arRaycastManager != null)
            {
                Log("✓ AR Raycast Manager configured");
            }
        }
        
        void VerifyConfiguration()
        {
            Log("Verifying configuration...");
            
            bool allComponentsFound = true;
            
            if (xrOrigin == null)
            {
                LogError("✗ XR Origin not found");
                allComponentsFound = false;
            }
            else
            {
                Log("✓ XR Origin found");
            }
            
            if (arSession == null)
            {
                LogError("✗ AR Session not found");
                allComponentsFound = false;
            }
            else
            {
                Log("✓ AR Session found");
            }
            
            if (arPlaneManager == null)
            {
                LogError("✗ AR Plane Manager not found");
                allComponentsFound = false;
            }
            else
            {
                Log("✓ AR Plane Manager found");
            }
            
            if (arRaycastManager == null)
            {
                LogError("✗ AR Raycast Manager not found");
                allComponentsFound = false;
            }
            else
            {
                Log("✓ AR Raycast Manager found");
            }
            
            if (arCameraManager == null)
            {
                LogError("✗ AR Camera Manager not found");
                allComponentsFound = false;
            }
            else
            {
                Log("✓ AR Camera Manager found");
            }
            
            if (allComponentsFound)
            {
                Log("✓ All AR Foundation components are properly configured!");
            }
            else
            {
                LogError("✗ Some components are missing. Please check the configuration.");
            }
        }
        
        [ContextMenu("Test XR Subsystems")]
        public void TestXRSubsystems()
        {
            Log("=== XR SUBSYSTEM TEST ===");
            
            // Test XR Session
            var sessionSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRSessionSubsystem>();
            if (sessionSubsystem != null)
            {
                Log($"✓ XRSessionSubsystem: {sessionSubsystem.running}");
            }
            else
            {
                LogError("✗ XRSessionSubsystem not available");
            }
            
            // Test XR Input
            var inputSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRInputSubsystem>();
            if (inputSubsystem != null)
            {
                Log($"✓ XRInputSubsystem: {inputSubsystem.running}");
            }
            else
            {
                LogError("✗ XRInputSubsystem not available");
            }
            
            // Test AR Camera
            var cameraSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRCameraSubsystem>();
            if (cameraSubsystem != null)
            {
                Log($"✓ XRCameraSubsystem: {cameraSubsystem.running}");
            }
            else
            {
                LogError("✗ XRCameraSubsystem not available");
            }
            
            // Test AR Plane
            var planeSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRPlaneSubsystem>();
            if (planeSubsystem != null)
            {
                Log($"✓ XRPlaneSubsystem: {planeSubsystem.running}");
            }
            else
            {
                LogError("✗ XRPlaneSubsystem not available");
            }
        }
        
        void Log(string message)
        {
            if (enableDebugLogging)
            {
                Debug.Log($"[XRConfigurationFixer] {message}");
            }
        }
        
        void LogWarning(string message)
        {
            if (enableDebugLogging)
            {
                Debug.LogWarning($"[XRConfigurationFixer] {message}");
            }
        }
        
        void LogError(string message)
        {
            if (enableDebugLogging)
            {
                Debug.LogError($"[XRConfigurationFixer] {message}");
            }
        }
    }
}
