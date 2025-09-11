using UnityEngine;
using UnityEngine.UI;

namespace ConvaiMR
{
    /// <summary>
    /// Simple script to hide common UI elements at startup, keeping only the avatar visible
    /// </summary>
    public class SimpleUIManager : MonoBehaviour
    {
        [Header("UI Management")]
        [Tooltip("Hide UI elements at startup")]
        public bool hideUIAtStart = true;
        
        [Tooltip("Delay before hiding UI (seconds)")]
        public float hideDelay = 1.5f;
        
        [Tooltip("Show debug messages")]
        public bool showDebug = true;

        void Start()
        {
            if (hideUIAtStart)
            {
                Invoke(nameof(HideCommonUIElements), hideDelay);
            }
        }

        void HideCommonUIElements()
        {
            if (showDebug)
                Debug.Log("[SimpleUIManager] Hiding common UI elements to focus on avatar...");
            
            // Hide common UI elements by name
            HideUIElement("Coaching UI");
            HideUIElement("Spatial Panel Scroll");
            HideUIElement("Spatial Panel Manipulator");
            HideUIElement("Hand Menu Setup");
            HideUIElement("Tutorial");
            HideUIElement("Instructions");
            HideUIElement("Help");
            HideUIElement("Settings");
            HideUIElement("Menu");
            
            // Hide Canvas elements that are not essential
            HideNonEssentialCanvases();
            
            if (showDebug)
                Debug.Log("[SimpleUIManager] UI cleanup completed. Avatar should now be the main focus.");
        }

        void HideUIElement(string elementName)
        {
            GameObject obj = GameObject.Find(elementName);
            if (obj != null)
            {
                obj.SetActive(false);
                if (showDebug)
                    Debug.Log($"[SimpleUIManager] Hidden: {elementName}");
            }
        }

        void HideNonEssentialCanvases()
        {
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();
            
            foreach (Canvas canvas in allCanvases)
            {
                // Keep Convai Transcript UI and permission dialogs
                if (canvas.name.Contains("Convai Transcript") || 
                    canvas.name.Contains("Permission") ||
                    canvas.name.Contains("Dialog"))
                {
                    continue;
                }
                
                // Hide other canvases
                canvas.gameObject.SetActive(false);
                if (showDebug)
                    Debug.Log($"[SimpleUIManager] Hidden Canvas: {canvas.name}");
            }
        }

        /// <summary>
        /// Show all UI elements (for debugging)
        /// </summary>
        [ContextMenu("Show All UI")]
        public void ShowAllUI()
        {
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in allCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
            
            if (showDebug)
                Debug.Log("[SimpleUIManager] All UI elements restored");
        }

        /// <summary>
        /// Hide UI elements again
        /// </summary>
        [ContextMenu("Hide UI Again")]
        public void HideUIAgain()
        {
            HideCommonUIElements();
        }
    }
}
