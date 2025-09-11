using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using System.Collections.Generic;

namespace ConvaiMR
{
    /// <summary>
    /// Automatically manages UI to prioritize the avatar at startup
    /// Hides common MR UI elements and keeps only essential permissions
    /// </summary>
    public class AvatarFirstUIManager : MonoBehaviour
    {
        [Header("Auto UI Management")]
        [Tooltip("Automatically find and hide common UI elements")]
        public bool autoHideUI = true;
        
        [Tooltip("Delay before hiding UI elements (seconds)")]
        public float hideDelay = 2.0f;
        
        [Tooltip("Show debug messages")]
        public bool showDebugMessages = true;

        [Header("UI Element Patterns to Hide")]
        [Tooltip("Common UI element names to hide (partial matches)")]
        public string[] uiPatternsToHide = {
            "Coaching UI",
            "Spatial Panel",
            "Hand Menu",
            "Tutorial",
            "Instructions",
            "Help Panel",
            "Settings Panel",
            "Menu Panel",
            "UI Panel",
            "Canvas",
            "Button Panel",
            "Control Panel"
        };

        [Header("Essential UI to Keep")]
        [Tooltip("UI elements that should remain visible")]
        public string[] essentialUIPatterns = {
            "Permission",
            "Dialog",
            "Alert",
            "Warning",
            "Error",
            "Convai Transcript"
        };

        private List<GameObject> hiddenUIElements = new List<GameObject>();

        void Start()
        {
            if (autoHideUI)
            {
                Invoke(nameof(ManageStartupUI), hideDelay);
            }
            
            if (showDebugMessages)
                Debug.Log("[AvatarFirstUIManager] Avatar-first UI manager initialized");
        }

        void ManageStartupUI()
        {
            if (showDebugMessages)
                Debug.Log("[AvatarFirstUIManager] Managing startup UI for avatar-first experience...");
            
            // Find and hide unnecessary UI elements
            HideUnnecessaryUIElements();
            
            // Ensure essential UI remains visible
            EnsureEssentialUIVisible();
            
            // Log results
            if (showDebugMessages)
            {
                Debug.Log($"[AvatarFirstUIManager] Hidden {hiddenUIElements.Count} UI elements");
                Debug.Log("[AvatarFirstUIManager] Avatar should now be the primary focus");
            }
        }

        void HideUnnecessaryUIElements()
        {
            // Find all Canvas objects in the scene
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();
            
            foreach (Canvas canvas in allCanvases)
            {
                // Skip if this is an essential UI
                if (IsEssentialUI(canvas.gameObject))
                    continue;
                
                // Check if this canvas should be hidden
                if (ShouldHideUIElement(canvas.gameObject))
                {
                    canvas.gameObject.SetActive(false);
                    hiddenUIElements.Add(canvas.gameObject);
                    
                    if (showDebugMessages)
                        Debug.Log($"[AvatarFirstUIManager] Hidden Canvas: {canvas.gameObject.name}");
                }
            }
            
            // Also check for individual UI elements
            GameObject[] allUIObjects = FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in allUIObjects)
            {
                // Skip if already processed or if it's essential
                if (hiddenUIElements.Contains(obj) || IsEssentialUI(obj))
                    continue;
                
                // Check if this object should be hidden
                if (ShouldHideUIElement(obj) && HasUIComponent(obj))
                {
                    obj.SetActive(false);
                    hiddenUIElements.Add(obj);
                    
                    if (showDebugMessages)
                        Debug.Log($"[AvatarFirstUIManager] Hidden UI Element: {obj.name}");
                }
            }
        }

        bool ShouldHideUIElement(GameObject obj)
        {
            string objName = obj.name.ToLower();
            
            foreach (string pattern in uiPatternsToHide)
            {
                if (objName.Contains(pattern.ToLower()))
                {
                    return true;
                }
            }
            
            return false;
        }

        bool IsEssentialUI(GameObject obj)
        {
            string objName = obj.name.ToLower();
            
            foreach (string pattern in essentialUIPatterns)
            {
                if (objName.Contains(pattern.ToLower()))
                {
                    return true;
                }
            }
            
            return false;
        }

        bool HasUIComponent(GameObject obj)
        {
            return obj.GetComponent<Canvas>() != null ||
                   obj.GetComponent<Button>() != null ||
                   obj.GetComponent<Image>() != null ||
                   obj.GetComponent<Text>() != null ||
                   obj.GetComponent<TextMeshProUGUI>() != null ||
                   obj.GetComponent<Slider>() != null ||
                   obj.GetComponent<Toggle>() != null;
        }

        void EnsureEssentialUIVisible()
        {
            // Find and ensure essential UI elements are visible
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in allObjects)
            {
                if (IsEssentialUI(obj))
                {
                    obj.SetActive(true);
                    
                    if (showDebugMessages)
                        Debug.Log($"[AvatarFirstUIManager] Ensured visible: {obj.name}");
                }
            }
        }

        /// <summary>
        /// Restore all hidden UI elements (for debugging)
        /// </summary>
        [ContextMenu("Restore All UI")]
        public void RestoreAllUI()
        {
            foreach (GameObject obj in hiddenUIElements)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            
            hiddenUIElements.Clear();
            
            if (showDebugMessages)
                Debug.Log("[AvatarFirstUIManager] All UI elements restored");
        }

        /// <summary>
        /// Hide UI elements again
        /// </summary>
        [ContextMenu("Hide UI Again")]
        public void HideUIAgain()
        {
            hiddenUIElements.Clear();
            ManageStartupUI();
        }

        /// <summary>
        /// Toggle UI visibility
        /// </summary>
        [ContextMenu("Toggle UI")]
        public void ToggleUI()
        {
            if (hiddenUIElements.Count > 0)
            {
                RestoreAllUI();
            }
            else
            {
                HideUIAgain();
            }
        }

        void OnDestroy()
        {
            // Clean up
            hiddenUIElements.Clear();
        }
    }
}
