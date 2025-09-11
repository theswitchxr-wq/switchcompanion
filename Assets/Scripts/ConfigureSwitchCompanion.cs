using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// Simple script to configure Switch Companion directly on the ConvaiNPC
    /// </summary>
    public class ConfigureSwitchCompanion : MonoBehaviour
    {
        [Header("Switch Companion Configuration")]
        [Tooltip("Apply configuration on start")]
        public bool applyOnStart = true;
        
        [Tooltip("Show debug messages")]
        public bool showDebug = true;

        void Start()
        {
            if (applyOnStart)
            {
                ConfigureCharacter();
            }
        }

        /// <summary>
        /// Configures the character as Switch Companion
        /// </summary>
        [ContextMenu("Configure as Switch Companion")]
        public void ConfigureCharacter()
        {
            ConvaiNPC convaiNPC = GetComponent<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("[ConfigureSwitchCompanion] No ConvaiNPC component found!");
                return;
            }

            // Set character identity
            convaiNPC.characterName = "Switch Companion";
            convaiNPC.characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
            
            // Enable features for better interaction
            convaiNPC.LipSync = true;
            convaiNPC.HeadEyeTracking = true;
            convaiNPC.EyeBlinking = true;
            convaiNPC.NarrativeDesignManager = true;
            
            if (showDebug)
            {
                Debug.Log("[ConfigureSwitchCompanion] Character configured as Switch Companion");
                Debug.Log($"[ConfigureSwitchCompanion] Name: {convaiNPC.characterName}");
                Debug.Log($"[ConfigureSwitchCompanion] ID: {convaiNPC.characterID}");
                Debug.Log("[ConfigureSwitchCompanion] Features enabled: LipSync, HeadEyeTracking, EyeBlinking, NarrativeDesignManager");
            }
        }

        /// <summary>
        /// Resets character to original settings
        /// </summary>
        [ContextMenu("Reset to Original")]
        public void ResetCharacter()
        {
            ConvaiNPC convaiNPC = GetComponent<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("[ConfigureSwitchCompanion] No ConvaiNPC component found!");
                return;
            }

            // Reset to original settings
            convaiNPC.characterName = "Amelia";
            convaiNPC.characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
            
            if (showDebug)
            {
                Debug.Log("[ConfigureSwitchCompanion] Character reset to original settings");
            }
        }
    }
}

