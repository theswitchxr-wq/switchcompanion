using UnityEngine;
using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.Features.LipSync;

/// <summary>
/// Automatically sets up Convai avatar features for proper voice interaction
/// </summary>
public class ConvaiAutoSetup : MonoBehaviour
{
    void Start()
    {
        Debug.Log("[ConvaiAutoSetup] Setting up Convai avatar features...");
        
        // Find and configure the Convai NPC
        var convaiNPC = FindObjectOfType<ConvaiNPC>();
        if (convaiNPC != null)
        {
            convaiNPC.LipSync = true;
            convaiNPC.HeadEyeTracking = true;
            convaiNPC.EyeBlinking = true;
            Debug.Log("[ConvaiAutoSetup] Enabled Convai features: LipSync, HeadEyeTracking, EyeBlinking");
        }
        
        // Find and configure head tracking
        var headTracking = FindObjectOfType<ConvaiHeadTracking>();
        if (headTracking != null)
        {
            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                headTracking.TargetObject = mainCamera.transform;
                Debug.Log("[ConvaiAutoSetup] Set Main Camera as head tracking target");
            }
        }
        
        // Find and configure lip sync
        var lipSync = FindObjectOfType<ConvaiLipSync>();
        if (lipSync != null)
        {
            // Lip sync should be automatically configured
            Debug.Log("[ConvaiAutoSetup] ConvaiLipSync found and ready");
        }
        
        Debug.Log("[ConvaiAutoSetup] Convai avatar setup completed!");
    }
}