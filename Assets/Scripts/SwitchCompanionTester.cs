using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// Test script to verify Switch Companion configuration
    /// </summary>
    public class SwitchCompanionTester : MonoBehaviour
    {
        [Header("Test Configuration")]
        [Tooltip("Show detailed test results")]
        public bool showDetailedResults = true;
        
        [Tooltip("Test on start")]
        public bool testOnStart = true;

        void Start()
        {
            if (testOnStart)
            {
                TestSwitchCompanionConfiguration();
            }
        }

        /// <summary>
        /// Tests the Switch Companion configuration
        /// </summary>
        [ContextMenu("Test Switch Companion")]
        public void TestSwitchCompanionConfiguration()
        {
            Debug.Log("=== Switch Companion Configuration Test ===");
            
            // Find the Convai NPC
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("❌ No ConvaiNPC found in scene!");
                return;
            }

            // Test basic configuration
            TestBasicConfiguration(convaiNPC);
            
            // Test advanced features
            TestAdvancedFeatures(convaiNPC);
            
            // Test input system
            TestInputSystem();
            
            Debug.Log("=== Test Complete ===");
        }

        void TestBasicConfiguration(ConvaiNPC convaiNPC)
        {
            Debug.Log("--- Basic Configuration Test ---");
            
            // Test character name
            if (convaiNPC.characterName == "Switch Companion")
            {
                Debug.Log("✅ Character name: " + convaiNPC.characterName);
            }
            else
            {
                Debug.LogWarning("⚠️ Character name mismatch. Expected: Switch Companion, Got: " + convaiNPC.characterName);
            }
            
            // Test character ID
            if (!string.IsNullOrEmpty(convaiNPC.characterID))
            {
                Debug.Log("✅ Character ID: " + convaiNPC.characterID);
            }
            else
            {
                Debug.LogError("❌ Character ID is empty!");
            }
            
            // Test if character is active
            if (convaiNPC.isCharacterActive)
            {
                Debug.Log("✅ Character is active");
            }
            else
            {
                Debug.LogWarning("⚠️ Character is not active");
            }
        }

        void TestAdvancedFeatures(ConvaiNPC convaiNPC)
        {
            Debug.Log("--- Advanced Features Test ---");
            
            // Test Lip Sync
            if (convaiNPC.LipSync)
            {
                Debug.Log("✅ Lip Sync enabled");
            }
            else
            {
                Debug.LogWarning("⚠️ Lip Sync disabled");
            }
            
            // Test Head & Eye Tracking
            if (convaiNPC.HeadEyeTracking)
            {
                Debug.Log("✅ Head & Eye Tracking enabled");
            }
            else
            {
                Debug.LogWarning("⚠️ Head & Eye Tracking disabled");
            }
            
            // Test Eye Blinking
            if (convaiNPC.EyeBlinking)
            {
                Debug.Log("✅ Eye Blinking enabled");
            }
            else
            {
                Debug.LogWarning("⚠️ Eye Blinking disabled");
            }
            
            // Test Narrative Design Manager
            if (convaiNPC.NarrativeDesignManager)
            {
                Debug.Log("✅ Narrative Design Manager enabled");
            }
            else
            {
                Debug.LogWarning("⚠️ Narrative Design Manager disabled");
            }
        }

        void TestInputSystem()
        {
            Debug.Log("--- Input System Test ---");
            
            // Test if Input System is available
            if (UnityEngine.InputSystem.InputSystem.devices.Count > 0)
            {
                Debug.Log("✅ Input System devices found: " + UnityEngine.InputSystem.InputSystem.devices.Count);
                
                // List available devices
                foreach (var device in UnityEngine.InputSystem.InputSystem.devices)
                {
                    Debug.Log("  - " + device.name + " (" + device.GetType().Name + ")");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ No Input System devices found");
            }
            
            // Test microphone availability
            if (Microphone.devices.Length > 0)
            {
                Debug.Log("✅ Microphone devices found: " + Microphone.devices.Length);
                foreach (string device in Microphone.devices)
                {
                    Debug.Log("  - " + device);
                }
            }
            else
            {
                Debug.LogWarning("⚠️ No microphone devices found");
            }
        }

        /// <summary>
        /// Tests the character's response to a sample input
        /// </summary>
        [ContextMenu("Test Character Response")]
        public void TestCharacterResponse()
        {
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("❌ No ConvaiNPC found for response test!");
                return;
            }

            Debug.Log("--- Character Response Test ---");
            Debug.Log("Testing character response...");
            Debug.Log("Expected: Switch Companion should respond warmly and introduce herself");
            Debug.Log("Try saying: 'Hello, what's your name?'");
            Debug.Log("The character should respond with her Switch Companion personality");
        }

        /// <summary>
        /// Resets the character to default settings
        /// </summary>
        [ContextMenu("Reset Character")]
        public void ResetCharacter()
        {
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("❌ No ConvaiNPC found for reset!");
                return;
            }

            // Reset to original settings
            convaiNPC.characterName = "Amelia";
            convaiNPC.LipSync = false;
            convaiNPC.HeadEyeTracking = false;
            convaiNPC.EyeBlinking = false;
            convaiNPC.NarrativeDesignManager = false;
            
            Debug.Log("✅ Character reset to original settings");
        }

        /// <summary>
        /// Reconfigures the character as Switch Companion
        /// </summary>
        [ContextMenu("Reconfigure as Switch Companion")]
        public void ReconfigureAsSwitchCompanion()
        {
            ConvaiNPC convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            if (convaiNPC == null)
            {
                Debug.LogError("❌ No ConvaiNPC found for reconfiguration!");
                return;
            }

            // Apply Switch Companion configuration
            convaiNPC.characterName = "Switch Companion";
            convaiNPC.LipSync = true;
            convaiNPC.HeadEyeTracking = true;
            convaiNPC.EyeBlinking = true;
            convaiNPC.NarrativeDesignManager = true;
            
            Debug.Log("✅ Character reconfigured as Switch Companion");
        }
    }
}

