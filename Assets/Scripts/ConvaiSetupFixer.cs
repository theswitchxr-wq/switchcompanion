using UnityEngine;
using Convai.Scripts.Runtime.Core;
using Convai.Scripts.Runtime.Features.LipSync;
using ConvaiMR;

/// <summary>
/// Fixes critical Convai avatar setup issues for proper voice interaction
/// </summary>
public class ConvaiSetupFixer : MonoBehaviour
{
    [Header("References")]
    public ConvaiMRMicrophoneInput microphoneInput;
    public ConvaiNPC convaiNPC;
    public ConvaiHeadTracking headTracking;
    public ConvaiLipSync lipSync;
    public AudioSource avatarAudioSource;
    
    [Header("Input Actions")]
    public UnityEngine.InputSystem.InputActionAsset questInputActions;
    
    void Start()
    {
        Debug.Log("[ConvaiSetupFixer] Starting Convai avatar setup fix...");
        
        // Fix microphone input connections
        FixMicrophoneInput();
        
        // Fix Convai NPC features
        FixConvaiFeatures();
        
        // Fix head tracking
        FixHeadTracking();
        
        // Fix input actions
        FixInputActions();
        
        Debug.Log("[ConvaiSetupFixer] Convai avatar setup completed!");
    }
    
    void FixMicrophoneInput()
    {
        if (microphoneInput == null)
        {
            microphoneInput = FindObjectOfType<ConvaiMRMicrophoneInput>();
        }
        
        if (microphoneInput != null)
        {
            // The new microphone input script handles AudioSource connection automatically
            Debug.Log("[ConvaiSetupFixer] Microphone input found - AudioSource connection handled automatically");
        }
        else
        {
            Debug.LogError("[ConvaiSetupFixer] ConvaiMRMicrophoneInput not found!");
        }
    }
    
    void FixConvaiFeatures()
    {
        if (convaiNPC == null)
        {
            convaiNPC = FindObjectOfType<ConvaiNPC>();
        }
        
        if (convaiNPC != null)
        {
            // Enable all Convai features
            convaiNPC.LipSync = true;
            convaiNPC.HeadEyeTracking = true;
            convaiNPC.EyeBlinking = true;
            
            Debug.Log("[ConvaiSetupFixer] Enabled Convai features: LipSync, HeadEyeTracking, EyeBlinking");
        }
        else
        {
            Debug.LogError("[ConvaiSetupFixer] ConvaiNPC not found!");
        }
    }
    
    void FixHeadTracking()
    {
        if (headTracking == null)
        {
            headTracking = FindObjectOfType<ConvaiHeadTracking>();
        }
        
        if (headTracking != null)
        {
            // Set the main camera as the head tracking target
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                headTracking.TargetObject = mainCamera.transform;
                Debug.Log("[ConvaiSetupFixer] Set Main Camera as head tracking target");
            }
            else
            {
                Debug.LogWarning("[ConvaiSetupFixer] Main Camera not found!");
            }
        }
        else
        {
            Debug.LogError("[ConvaiSetupFixer] ConvaiHeadTracking not found!");
        }
    }
    
    void FixInputActions()
    {
        if (microphoneInput == null) return;
        
        // Load the Quest input actions
        if (questInputActions == null)
        {
            questInputActions = Resources.Load<UnityEngine.InputSystem.InputActionAsset>("ConvaiQuestControls");
            if (questInputActions == null)
            {
                Debug.LogWarning("[ConvaiSetupFixer] Quest input actions not found in Resources!");
                return;
            }
        }
        
        // Enable the input actions
        questInputActions.Enable();
        
        // The new microphone input script handles input actions automatically
        Debug.Log("[ConvaiSetupFixer] Input actions handled automatically by microphone input script");
    }
    
    [ContextMenu("Fix All Connections")]
    public void FixAllConnections()
    {
        FixMicrophoneInput();
        FixConvaiFeatures();
        FixHeadTracking();
        FixInputActions();
    }
}
