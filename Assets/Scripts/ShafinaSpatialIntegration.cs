using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// Main integration script that automatically sets up Shafina's spatial awareness
    /// This script should be added to the scene to enable all spatial features
    /// </summary>
    public class ShafinaSpatialIntegration : MonoBehaviour
    {
        [Header("Integration Settings")]
        [SerializeField] private bool autoSetupOnAwake = true;
        [SerializeField] private bool enableDebugMode = true;
        [SerializeField] private bool showSetupProgress = true;
        
        [Header("Spatial Awareness Settings")]
        [SerializeField] private float detectionRange = 3f;
        [SerializeField] private float detectionInterval = 0.5f;
        [SerializeField] private float reactionCooldown = 5f;
        
        [Header("Visual Settings")]
        [SerializeField] private bool enableVisualIndicators = true;
        [SerializeField] private bool enableDebugRays = true;
        
        [Header("Audio Settings")]
        [SerializeField] private bool enableAudioReactions = true;
        [SerializeField] private AudioClip[] spatialReactionClips;
        
        // Private references
        private ShafinaSpatialSetup spatialSetup;
        private ShafinaSpatialAwareness spatialAwareness;
        private ShafinaSpatialDialogue spatialDialogue;
        private ConvaiNPC convaiNPC;
        
        void Awake()
        {
            if (autoSetupOnAwake)
            {
                SetupSpatialIntegration();
            }
        }
        
        void Start()
        {
            if (enableDebugMode)
            {
                LogDebugInfo();
            }
        }
        
        [ContextMenu("Setup Spatial Integration")]
        public void SetupSpatialIntegration()
        {
            Log("=== SHAFINA SPATIAL AWARENESS INTEGRATION ===");
            Log("Setting up complete spatial awareness system...");
            
            // Step 1: Find or create spatial setup component
            SetupSpatialSetupComponent();
            
            // Step 2: Find Convai avatar
            FindConvaiAvatar();
            
            // Step 3: Setup spatial awareness
            SetupSpatialAwareness();
            
            // Step 4: Setup dialogue integration
            SetupDialogueIntegration();
            
            // Step 5: Configure settings
            ConfigureSettings();
            
            // Step 6: Test the system
            TestSpatialSystem();
            
            Log("=== SPATIAL INTEGRATION COMPLETE ===");
            Log("Shafina is now spatially aware! Move around to see her react to the environment.");
        }
        
        void SetupSpatialSetupComponent()
        {
            spatialSetup = FindObjectOfType<ShafinaSpatialSetup>();
            if (spatialSetup == null)
            {
                GameObject setupGO = new GameObject("Shafina Spatial Setup");
                spatialSetup = setupGO.AddComponent<ShafinaSpatialSetup>();
                Log("Created ShafinaSpatialSetup component");
            }
            else
            {
                Log("Found existing ShafinaSpatialSetup component");
            }
        }
        
        void FindConvaiAvatar()
        {
            convaiNPC = FindObjectOfType<ConvaiNPC>();
            if (convaiNPC == null)
            {
                LogError("No ConvaiNPC found in scene! Please ensure there's a Convai avatar present.");
                return;
            }
            
            Log($"Found Convai avatar: {convaiNPC.name}");
        }
        
        void SetupSpatialAwareness()
        {
            if (convaiNPC == null) return;
            
            spatialAwareness = convaiNPC.GetComponent<ShafinaSpatialAwareness>();
            if (spatialAwareness == null)
            {
                spatialAwareness = convaiNPC.gameObject.AddComponent<ShafinaSpatialAwareness>();
                Log("Added ShafinaSpatialAwareness component to avatar");
            }
            else
            {
                Log("Found existing ShafinaSpatialAwareness component");
            }
        }
        
        void SetupDialogueIntegration()
        {
            if (convaiNPC == null) return;
            
            spatialDialogue = convaiNPC.GetComponent<ShafinaSpatialDialogue>();
            if (spatialDialogue == null)
            {
                spatialDialogue = convaiNPC.gameObject.AddComponent<ShafinaSpatialDialogue>();
                Log("Added ShafinaSpatialDialogue component to avatar");
            }
            else
            {
                Log("Found existing ShafinaSpatialDialogue component");
            }
        }
        
        void ConfigureSettings()
        {
            Log("Configuring spatial awareness settings...");
            
            // Configure spatial awareness
            if (spatialAwareness != null)
            {
                // Use reflection to set private fields
                SetPrivateField(spatialAwareness, "detectionRange", detectionRange);
                SetPrivateField(spatialAwareness, "detectionInterval", detectionInterval);
                SetPrivateField(spatialAwareness, "showDebugRays", enableDebugRays);
                SetPrivateField(spatialAwareness, "enableSpatialReactions", true);
                
                Log($"- Detection Range: {detectionRange}m");
                Log($"- Detection Interval: {detectionInterval}s");
                Log($"- Debug Rays: {enableDebugRays}");
            }
            
            // Configure dialogue integration
            if (spatialDialogue != null)
            {
                SetPrivateField(spatialDialogue, "reactionCooldown", reactionCooldown);
                SetPrivateField(spatialDialogue, "enableVoiceReactions", enableAudioReactions);
                SetPrivateField(spatialDialogue, "enableTextReactions", true);
                
                Log($"- Reaction Cooldown: {reactionCooldown}s");
                Log($"- Audio Reactions: {enableAudioReactions}");
            }
            
            Log("Settings configured successfully");
        }
        
        void TestSpatialSystem()
        {
            Log("Testing spatial awareness system...");
            
            if (spatialAwareness != null)
            {
                Log("✓ Spatial awareness component ready");
            }
            else
            {
                LogError("✗ Spatial awareness component missing");
            }
            
            if (spatialDialogue != null)
            {
                Log("✓ Dialogue integration ready");
            }
            else
            {
                LogError("✗ Dialogue integration missing");
            }
            
            if (convaiNPC != null)
            {
                Log("✓ Convai NPC found and ready");
            }
            else
            {
                LogError("✗ Convai NPC not found");
            }
            
            // Test manual reactions
            if (spatialDialogue != null)
            {
                Log("Testing manual reactions...");
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Wall);
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Floor);
            }
        }
        
        void LogDebugInfo()
        {
            Log("=== SPATIAL AWARENESS DEBUG INFO ===");
            Log($"Detection Range: {detectionRange}m");
            Log($"Detection Interval: {detectionInterval}s");
            Log($"Reaction Cooldown: {reactionCooldown}s");
            Log($"Visual Indicators: {enableVisualIndicators}");
            Log($"Debug Rays: {enableDebugRays}");
            Log($"Audio Reactions: {enableAudioReactions}");
            Log("=====================================");
        }
        
        void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }
        
        [ContextMenu("Test Spatial Reactions")]
        public void TestSpatialReactions()
        {
            if (spatialDialogue != null)
            {
                Log("Testing all spatial reactions...");
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Wall);
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Floor);
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Ceiling);
                spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Furniture);
            }
            else
            {
                LogError("Spatial dialogue component not found!");
            }
        }
        
        [ContextMenu("Clear All Spatial Data")]
        public void ClearAllSpatialData()
        {
            if (spatialAwareness != null)
            {
                spatialAwareness.ClearDetectedObjects();
                Log("Cleared all detected spatial objects");
            }
            
            if (spatialDialogue != null)
            {
                spatialDialogue.ClearReactionHistory();
                Log("Cleared reaction history");
            }
        }
        
        [ContextMenu("Show Spatial Status")]
        public void ShowSpatialStatus()
        {
            Log("=== SPATIAL AWARENESS STATUS ===");
            
            if (spatialAwareness != null)
            {
                var detectedObjects = spatialAwareness.GetDetectedObjects();
                Log($"Detected Objects: {detectedObjects.Count}");
                
                foreach (var obj in detectedObjects)
                {
                    Log($"- {obj.type} at {obj.position} (confidence: {obj.confidence:F2})");
                }
            }
            else
            {
                Log("Spatial awareness not available");
            }
            
            Log("===============================");
        }
        
        void Log(string message)
        {
            if (showSetupProgress)
            {
                Debug.Log($"[ShafinaSpatialIntegration] {message}");
            }
        }
        
        void LogError(string message)
        {
            if (showSetupProgress)
            {
                Debug.LogError($"[ShafinaSpatialIntegration] {message}");
            }
        }
    }
}
