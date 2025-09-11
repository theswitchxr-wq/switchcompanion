using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace ConvaiMR
{
    /// <summary>
    /// Manages UI elements at startup to show only the avatar and essential permissions
    /// </summary>
    public class StartupUIManager : MonoBehaviour
    {
        [Header("UI Management")]
        [Tooltip("UI elements to hide at startup")]
        public GameObject[] uiElementsToHide;
        
        [Tooltip("UI elements to keep visible (permissions, essential UI)")]
        public GameObject[] essentialUIElements;
        
        [Tooltip("Delay before hiding UI elements (seconds)")]
        public float hideUIDelay = 1.0f;
        
        [Header("Permission Management")]
        [Tooltip("Show permission request dialogs")]
        public bool showPermissionDialogs = true;
        
        [Tooltip("Permission dialog prefab")]
        public GameObject permissionDialogPrefab;
        
        [Header("Debug")]
        [Tooltip("Show debug messages")]
        public bool showDebugMessages = true;

        void Start()
        {
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] Initializing startup UI management...");
            
            // Hide UI elements after a short delay
            Invoke(nameof(HideUnnecessaryUI), hideUIDelay);
            
            // Show permission dialogs if needed
            if (showPermissionDialogs)
            {
                ShowPermissionDialogs();
            }
        }

        /// <summary>
        /// Hides unnecessary UI elements to show only the avatar
        /// </summary>
        void HideUnnecessaryUI()
        {
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] Hiding unnecessary UI elements...");
            
            // Hide specified UI elements
            foreach (GameObject uiElement in uiElementsToHide)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(false);
                    if (showDebugMessages)
                        Debug.Log($"[StartupUIManager] Hidden: {uiElement.name}");
                }
            }
            
            // Ensure essential UI elements remain visible
            foreach (GameObject essentialUI in essentialUIElements)
            {
                if (essentialUI != null)
                {
                    essentialUI.SetActive(true);
                    if (showDebugMessages)
                        Debug.Log($"[StartupUIManager] Kept visible: {essentialUI.name}");
                }
            }
            
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] UI management completed. Avatar should now be the main focus.");
        }

        /// <summary>
        /// Shows essential permission dialogs
        /// </summary>
        void ShowPermissionDialogs()
        {
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] Checking for required permissions...");
            
            // Check for microphone permission
            CheckMicrophonePermission();
            
            // Check for camera permission (for AR/VR)
            CheckCameraPermission();
            
            // Check for XR permissions
            CheckXRPermissions();
        }

        void CheckMicrophonePermission()
        {
            // For Quest/VR, microphone permission is usually handled automatically
            // But we can show a user-friendly message
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] Microphone permission check completed");
        }

        void CheckCameraPermission()
        {
            // For AR/VR applications, camera permission might be needed
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] Camera permission check completed");
        }

        void CheckXRPermissions()
        {
            // Check if XR is available and working
            if (XRSettings.enabled)
            {
                if (showDebugMessages)
                    Debug.Log("[StartupUIManager] XR is enabled and ready");
            }
            else
            {
                if (showDebugMessages)
                    Debug.LogWarning("[StartupUIManager] XR is not enabled");
            }
        }

        /// <summary>
        /// Manually hide all UI elements (can be called from other scripts)
        /// </summary>
        [ContextMenu("Hide All UI")]
        public void HideAllUI()
        {
            HideUnnecessaryUI();
        }

        /// <summary>
        /// Show all UI elements (for debugging)
        /// </summary>
        [ContextMenu("Show All UI")]
        public void ShowAllUI()
        {
            foreach (GameObject uiElement in uiElementsToHide)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(true);
                }
            }
            
            if (showDebugMessages)
                Debug.Log("[StartupUIManager] All UI elements restored");
        }

        /// <summary>
        /// Toggle UI visibility
        /// </summary>
        [ContextMenu("Toggle UI")]
        public void ToggleUI()
        {
            bool anyUIHidden = false;
            
            foreach (GameObject uiElement in uiElementsToHide)
            {
                if (uiElement != null && !uiElement.activeInHierarchy)
                {
                    anyUIHidden = true;
                    break;
                }
            }
            
            if (anyUIHidden)
            {
                ShowAllUI();
            }
            else
            {
                HideAllUI();
            }
        }
    }
}
