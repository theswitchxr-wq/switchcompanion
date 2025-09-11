using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// Simple setup script to automatically configure Switch Companion
    /// </summary>
    public class SwitchCompanionSetup : MonoBehaviour
    {
        [Header("Auto Setup")]
        [Tooltip("Automatically apply configuration on start")]
        public bool autoSetupOnStart = true;
        
        [Tooltip("Show debug messages during setup")]
        public bool showDebugMessages = true;

        void Start()
        {
            if (autoSetupOnStart)
            {
                SetupSwitchCompanion();
            }
        }

        /// <summary>
        /// Sets up Switch Companion character configuration
        /// </summary>
        public void SetupSwitchCompanion()
        {
            // Find the Convai NPC in the scene
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("[SwitchCompanionSetup] No ConvaiNPC found in scene!");
                return;
            }

            // Apply basic character configuration
            ApplyBasicConfiguration(convaiNPC);
            
            // Add advanced configuration component if not present
            AddAdvancedConfiguration(convaiNPC.gameObject);
            
            if (showDebugMessages)
            {
                Debug.Log("[SwitchCompanionSetup] Switch Companion configuration applied successfully!");
                Debug.Log($"[SwitchCompanionSetup] Character: {convaiNPC.characterName}");
                Debug.Log($"[SwitchCompanionSetup] Character ID: {convaiNPC.characterID}");
            }
        }

        void ApplyBasicConfiguration(ConvaiNPC convaiNPC)
        {
            // Set character identity
            convaiNPC.characterName = "Switch Companion";
            convaiNPC.characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
            
            // Enable important features
            convaiNPC.LipSync = true;
            convaiNPC.HeadEyeTracking = true;
            convaiNPC.EyeBlinking = true;
            
            if (showDebugMessages)
            {
                Debug.Log("[SwitchCompanionSetup] Basic configuration applied");
            }
        }

        void AddAdvancedConfiguration(GameObject npcObject)
        {
            // Add the advanced configuration component
            SwitchCompanionAdvanced advancedConfig = npcObject.GetComponent<SwitchCompanionAdvanced>();
            if (advancedConfig == null)
            {
                advancedConfig = npcObject.AddComponent<SwitchCompanionAdvanced>();
                
                if (showDebugMessages)
                {
                    Debug.Log("[SwitchCompanionSetup] Added SwitchCompanionAdvanced component");
                }
            }

            // Configure the advanced settings
            advancedConfig.characterName = "Switch Companion";
            advancedConfig.characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
            advancedConfig.affectionLevel = 0.8f;
            advancedConfig.professionalismLevel = 0.7f;
            advancedConfig.playfulnessLevel = 0.6f;
            advancedConfig.supportivenessLevel = 0.9f;
            advancedConfig.intelligenceLevel = 0.85f;
            advancedConfig.rememberPersonalDetails = true;
            advancedConfig.rememberConversationHistory = true;
            advancedConfig.rememberUserGoals = true;
            advancedConfig.useTermsOfEndearment = true;
            advancedConfig.askFollowUpQuestions = true;
            advancedConfig.provideEmotionalSupport = true;
        }

        /// <summary>
        /// Manually trigger setup (useful for testing)
        /// </summary>
        [ContextMenu("Setup Switch Companion")]
        public void ManualSetup()
        {
            SetupSwitchCompanion();
        }

        /// <summary>
        /// Reset character to default settings
        /// </summary>
        [ContextMenu("Reset Character")]
        public void ResetCharacter()
        {
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            if (convaiNPC != null)
            {
                convaiNPC.characterName = "Amelia";
                convaiNPC.characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
                
                // Remove advanced configuration
                SwitchCompanionAdvanced advancedConfig = convaiNPC.GetComponent<SwitchCompanionAdvanced>();
                if (advancedConfig != null)
                {
                    DestroyImmediate(advancedConfig);
                }
                
                if (showDebugMessages)
                {
                    Debug.Log("[SwitchCompanionSetup] Character reset to default");
                }
            }
        }
    }
}

