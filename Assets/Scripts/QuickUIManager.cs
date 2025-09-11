using UnityEngine;

public class QuickUIManager : MonoBehaviour
{
    [Header("UI Management")]
    public bool hideUIAtStart = true;
    public float hideDelay = 1.5f;
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
            Debug.Log("[QuickUIManager] Hiding common UI elements...");

        // Hide Hand Menu Setup
        GameObject handMenu = GameObject.Find("Hand Menu Setup");
        if (handMenu != null)
        {
            handMenu.SetActive(false);
            if (showDebug) Debug.Log("[QuickUIManager] Hidden Hand Menu Setup");
        }

        // Hide Convai Settings Panel
        GameObject settingsPanel = GameObject.Find("Convai Settings Panel");
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            if (showDebug) Debug.Log("[QuickUIManager] Hidden Convai Settings Panel");
        }

        // Hide Convai Canvas (contains crosshair and logo)
        GameObject convaiCanvas = GameObject.Find("Convai Canvas");
        if (convaiCanvas != null)
        {
            convaiCanvas.SetActive(false);
            if (showDebug) Debug.Log("[QuickUIManager] Hidden Convai Canvas");
        }

        // Hide Coaching UI (if active)
        GameObject coachingUI = GameObject.Find("Coaching UI");
        if (coachingUI != null)
        {
            coachingUI.SetActive(false);
            if (showDebug) Debug.Log("[QuickUIManager] Hidden Coaching UI");
        }

        // Hide Spatial Panel Manipulator (if active)
        GameObject spatialPanel = GameObject.Find("Spatial Panel Manipulator");
        if (spatialPanel != null)
        {
            spatialPanel.SetActive(false);
            if (showDebug) Debug.Log("[QuickUIManager] Hidden Spatial Panel Manipulator");
        }

        if (showDebug)
            Debug.Log("[QuickUIManager] UI hiding completed. Avatar should now be the main focus!");
    }

    [ContextMenu("Hide UI Now")]
    public void HideUINow()
    {
        HideCommonUIElements();
    }

    [ContextMenu("Show All UI")]
    public void ShowAllUI()
    {
        if (showDebug)
            Debug.Log("[QuickUIManager] Showing all UI elements...");

        // Show Hand Menu Setup
        GameObject handMenu = GameObject.Find("Hand Menu Setup");
        if (handMenu != null)
        {
            handMenu.SetActive(true);
            if (showDebug) Debug.Log("[QuickUIManager] Shown Hand Menu Setup");
        }

        // Show Convai Settings Panel
        GameObject settingsPanel = GameObject.Find("Convai Settings Panel");
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            if (showDebug) Debug.Log("[QuickUIManager] Shown Convai Settings Panel");
        }

        // Show Convai Canvas
        GameObject convaiCanvas = GameObject.Find("Convai Canvas");
        if (convaiCanvas != null)
        {
            convaiCanvas.SetActive(true);
            if (showDebug) Debug.Log("[QuickUIManager] Shown Convai Canvas");
        }

        if (showDebug)
            Debug.Log("[QuickUIManager] All UI elements shown.");
    }
}