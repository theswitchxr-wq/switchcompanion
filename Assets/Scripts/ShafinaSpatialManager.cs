using UnityEngine;
using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;

namespace ConvaiMR
{
    /// <summary>
    /// Central manager for Shafina's spatial awareness system
    /// Provides easy access to all spatial features and controls
    /// </summary>
    public class ShafinaSpatialManager : MonoBehaviour
    {
        [Header("System Status")]
        [SerializeField] private bool isSystemActive = false;
        [SerializeField] private bool isDetectionEnabled = true;
        [SerializeField] private bool isReactionEnabled = true;
        
        [Header("Performance")]
        [SerializeField] private int maxDetectedObjects = 50;
        [SerializeField] private float cleanupInterval = 10f;
        
        // Component references
        private ShafinaSpatialAwareness spatialAwareness;
        private ShafinaSpatialDialogue spatialDialogue;
        private ConvaiNPC convaiNPC;
        
        // Events
        public System.Action<ShafinaSpatialAwareness.SpatialObject> OnObjectDetected;
        public System.Action<ShafinaSpatialAwareness.SpatialObject> OnObjectLost;
        public System.Action<string> OnSpatialReaction;
        
        // Statistics
        private int totalObjectsDetected = 0;
        private int totalReactionsTriggered = 0;
        private float systemStartTime;
        
        void Start()
        {
            InitializeSystem();
        }
        
        void InitializeSystem()
        {
            // Find components
            spatialAwareness = FindObjectOfType<ShafinaSpatialAwareness>();
            spatialDialogue = FindObjectOfType<ShafinaSpatialDialogue>();
            convaiNPC = FindObjectOfType<ConvaiNPC>();
            
            // Initialize system
            if (spatialAwareness != null && spatialDialogue != null && convaiNPC != null)
            {
                isSystemActive = true;
                systemStartTime = Time.time;
                
                Debug.Log("[ShafinaSpatialManager] Spatial awareness system initialized successfully!");
                Debug.Log($"[ShafinaSpatialManager] Avatar: {convaiNPC.name}");
            }
            else
            {
                Debug.LogError("[ShafinaSpatialManager] Failed to initialize spatial awareness system!");
                Debug.LogError("Required components: ShafinaSpatialAwareness, ShafinaSpatialDialogue, ConvaiNPC");
            }
        }
        
        void Update()
        {
            if (!isSystemActive) return;
            
            // Cleanup old objects periodically
            if (Time.time % cleanupInterval < Time.deltaTime)
            {
                CleanupOldObjects();
            }
        }
        
        // Public API Methods
        
        /// <summary>
        /// Enable or disable the entire spatial awareness system
        /// </summary>
        public void SetSystemActive(bool active)
        {
            isSystemActive = active;
            
            if (spatialAwareness != null)
            {
                spatialAwareness.enabled = active;
            }
            
            if (spatialDialogue != null)
            {
                spatialDialogue.enabled = active;
            }
            
            Debug.Log($"[ShafinaSpatialManager] System {(active ? "ENABLED" : "DISABLED")}");
        }
        
        /// <summary>
        /// Enable or disable object detection
        /// </summary>
        public void SetDetectionEnabled(bool enabled)
        {
            isDetectionEnabled = enabled;
            
            if (spatialAwareness != null)
            {
                // Use reflection to control detection
                var detectionField = typeof(ShafinaSpatialAwareness).GetField("detectionInterval", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (detectionField != null)
                {
                    detectionField.SetValue(spatialAwareness, enabled ? 0.5f : 0f);
                }
            }
            
            Debug.Log($"[ShafinaSpatialManager] Detection {(enabled ? "ENABLED" : "DISABLED")}");
        }
        
        /// <summary>
        /// Enable or disable spatial reactions
        /// </summary>
        public void SetReactionEnabled(bool enabled)
        {
            isReactionEnabled = enabled;
            
            if (spatialDialogue != null)
            {
                // Use reflection to control reactions
                var reactionField = typeof(ShafinaSpatialDialogue).GetField("enableVoiceReactions", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (reactionField != null)
                {
                    reactionField.SetValue(spatialDialogue, enabled);
                }
            }
            
            Debug.Log($"[ShafinaSpatialManager] Reactions {(enabled ? "ENABLED" : "DISABLED")}");
        }
        
        /// <summary>
        /// Get all currently detected spatial objects
        /// </summary>
        public List<ShafinaSpatialAwareness.SpatialObject> GetDetectedObjects()
        {
            if (spatialAwareness != null)
            {
                return spatialAwareness.GetDetectedObjects();
            }
            return new List<ShafinaSpatialAwareness.SpatialObject>();
        }
        
        /// <summary>
        /// Get objects of a specific type
        /// </summary>
        public List<ShafinaSpatialAwareness.SpatialObject> GetObjectsOfType(ShafinaSpatialAwareness.SpatialObjectType type)
        {
            if (spatialAwareness != null)
            {
                return spatialAwareness.GetObjectsOfType(type);
            }
            return new List<ShafinaSpatialAwareness.SpatialObject>();
        }
        
        /// <summary>
        /// Trigger a manual spatial reaction
        /// </summary>
        public void TriggerReaction(ShafinaSpatialAwareness.SpatialObjectType objectType)
        {
            if (spatialDialogue != null && isReactionEnabled)
            {
                spatialDialogue.TriggerManualReaction(objectType);
                totalReactionsTriggered++;
                
                OnSpatialReaction?.Invoke($"Manual reaction triggered for {objectType}");
            }
        }
        
        /// <summary>
        /// Clear all detected objects
        /// </summary>
        public void ClearAllObjects()
        {
            if (spatialAwareness != null)
            {
                spatialAwareness.ClearDetectedObjects();
                totalObjectsDetected = 0;
                Debug.Log("[ShafinaSpatialManager] All objects cleared");
            }
        }
        
        /// <summary>
        /// Get system statistics
        /// </summary>
        public SpatialSystemStats GetSystemStats()
        {
            return new SpatialSystemStats
            {
                isSystemActive = isSystemActive,
                isDetectionEnabled = isDetectionEnabled,
                isReactionEnabled = isReactionEnabled,
                totalObjectsDetected = totalObjectsDetected,
                totalReactionsTriggered = totalReactionsTriggered,
                currentObjectCount = GetDetectedObjects().Count,
                systemUptime = Time.time - systemStartTime
            };
        }
        
        /// <summary>
        /// Set detection range
        /// </summary>
        public void SetDetectionRange(float range)
        {
            if (spatialAwareness != null)
            {
                var rangeField = typeof(ShafinaSpatialAwareness).GetField("detectionRange", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (rangeField != null)
                {
                    rangeField.SetValue(spatialAwareness, range);
                    Debug.Log($"[ShafinaSpatialManager] Detection range set to {range}m");
                }
            }
        }
        
        /// <summary>
        /// Set reaction cooldown
        /// </summary>
        public void SetReactionCooldown(float cooldown)
        {
            if (spatialDialogue != null)
            {
                spatialDialogue.SetReactionCooldown(cooldown);
                Debug.Log($"[ShafinaSpatialManager] Reaction cooldown set to {cooldown}s");
            }
        }
        
        void CleanupOldObjects()
        {
            if (spatialAwareness != null)
            {
                var objects = GetDetectedObjects();
                if (objects.Count > maxDetectedObjects)
                {
                    // Remove oldest objects (this would need to be implemented in ShafinaSpatialAwareness)
                    Debug.Log($"[ShafinaSpatialManager] Object count ({objects.Count}) exceeds limit ({maxDetectedObjects})");
                }
            }
        }
        
        // Context menu methods for easy testing
        
        [ContextMenu("Test All Reactions")]
        public void TestAllReactions()
        {
            Debug.Log("[ShafinaSpatialManager] Testing all spatial reactions...");
            TriggerReaction(ShafinaSpatialAwareness.SpatialObjectType.Wall);
            TriggerReaction(ShafinaSpatialAwareness.SpatialObjectType.Floor);
            TriggerReaction(ShafinaSpatialAwareness.SpatialObjectType.Ceiling);
            TriggerReaction(ShafinaSpatialAwareness.SpatialObjectType.Furniture);
        }
        
        [ContextMenu("Show System Status")]
        public void ShowSystemStatus()
        {
            var stats = GetSystemStats();
            Debug.Log($"[ShafinaSpatialManager] === SYSTEM STATUS ===");
            Debug.Log($"System Active: {stats.isSystemActive}");
            Debug.Log($"Detection Enabled: {stats.isDetectionEnabled}");
            Debug.Log($"Reaction Enabled: {stats.isReactionEnabled}");
            Debug.Log($"Current Objects: {stats.currentObjectCount}");
            Debug.Log($"Total Detected: {stats.totalObjectsDetected}");
            Debug.Log($"Total Reactions: {stats.totalReactionsTriggered}");
            Debug.Log($"Uptime: {stats.systemUptime:F1}s");
            Debug.Log($"=========================");
        }
        
        [ContextMenu("Reset System")]
        public void ResetSystem()
        {
            ClearAllObjects();
            if (spatialDialogue != null)
            {
                spatialDialogue.ClearReactionHistory();
            }
            totalObjectsDetected = 0;
            totalReactionsTriggered = 0;
            systemStartTime = Time.time;
            Debug.Log("[ShafinaSpatialManager] System reset");
        }
        
        // Data structures
        
        [System.Serializable]
        public class SpatialSystemStats
        {
            public bool isSystemActive;
            public bool isDetectionEnabled;
            public bool isReactionEnabled;
            public int totalObjectsDetected;
            public int totalReactionsTriggered;
            public int currentObjectCount;
            public float systemUptime;
        }
    }
}
