using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// One-click setup script for Shafina's spatial awareness system
    /// Automatically configures all necessary components and prefabs
    /// </summary>
    public class ShafinaSpatialSetup : MonoBehaviour
    {
        [Header("Auto Setup")]
        [SerializeField] private bool autoSetupOnStart = true;
        [SerializeField] private bool createMissingComponents = true;
        
        [Header("Prefab References")]
        [SerializeField] private GameObject spatialIndicatorPrefab;
        [SerializeField] private Material wallMaterial;
        [SerializeField] private Material floorMaterial;
        [SerializeField] private Material furnitureMaterial;
        
        [Header("Debug")]
        [SerializeField] private bool showSetupLogs = true;
        
        void Start()
        {
            if (autoSetupOnStart)
            {
                SetupSpatialAwareness();
            }
        }
        
        [ContextMenu("Setup Spatial Awareness")]
        public void SetupSpatialAwareness()
        {
            Log("Starting Shafina Spatial Awareness Setup...");
            
            // Find or create Convai avatar
            GameObject avatar = FindOrCreateAvatar();
            if (avatar == null)
            {
                LogError("No avatar found! Please ensure there's a Convai NPC in the scene.");
                return;
            }
            
            // Setup AR Foundation
            SetupARFoundation();
            
            // Setup spatial awareness on avatar
            SetupAvatarSpatialAwareness(avatar);
            
            // Setup dialogue integration
            SetupDialogueIntegration(avatar);
            
            // Load materials and prefabs
            LoadMaterialsAndPrefabs();
            
            Log("Shafina Spatial Awareness Setup completed successfully!");
        }
        
        GameObject FindOrCreateAvatar()
        {
            // Look for existing Convai NPC
            ConvaiNPC existingNPC = FindObjectOfType<ConvaiNPC>();
            if (existingNPC != null)
            {
                Log($"Found existing Convai NPC: {existingNPC.name}");
                return existingNPC.gameObject;
            }
            
            // Look for avatar with "Shafina" in the name
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("shafina") || obj.name.ToLower().Contains("ninja"))
                {
                    ConvaiNPC npc = obj.GetComponent<ConvaiNPC>();
                    if (npc != null)
                    {
                        Log($"Found avatar with Convai NPC: {obj.name}");
                        return obj;
                    }
                }
            }
            
            LogWarning("No Convai avatar found. Please add a Convai NPC to the scene first.");
            return null;
        }
        
        void SetupARFoundation()
        {
            Log("Setting up AR Foundation...");
            
            // Create AR Foundation setup component
            ARFoundationSetup arSetup = FindObjectOfType<ARFoundationSetup>();
            if (arSetup == null && createMissingComponents)
            {
                GameObject arSetupGO = new GameObject("AR Foundation Setup");
                arSetup = arSetupGO.AddComponent<ARFoundationSetup>();
                Log("Created AR Foundation Setup component");
            }
            
            if (arSetup != null)
            {
                // The ARFoundationSetup component will handle the rest
                Log("AR Foundation setup component found/created");
            }
        }
        
        void SetupAvatarSpatialAwareness(GameObject avatar)
        {
            Log($"Setting up spatial awareness for {avatar.name}...");
            
            // Add ShafinaSpatialAwareness component
            ShafinaSpatialAwareness spatialAwareness = avatar.GetComponent<ShafinaSpatialAwareness>();
            if (spatialAwareness == null && createMissingComponents)
            {
                spatialAwareness = avatar.AddComponent<ShafinaSpatialAwareness>();
                Log("Added ShafinaSpatialAwareness component to avatar");
            }
            
            if (spatialAwareness != null)
            {
                // Configure spatial awareness settings
                ConfigureSpatialAwareness(spatialAwareness);
            }
        }
        
        void SetupDialogueIntegration(GameObject avatar)
        {
            Log("Setting up dialogue integration...");
            
            // Add ShafinaSpatialDialogue component
            ShafinaSpatialDialogue spatialDialogue = avatar.GetComponent<ShafinaSpatialDialogue>();
            if (spatialDialogue == null && createMissingComponents)
            {
                spatialDialogue = avatar.AddComponent<ShafinaSpatialDialogue>();
                Log("Added ShafinaSpatialDialogue component to avatar");
            }
            
            if (spatialDialogue != null)
            {
                Log("Spatial dialogue integration configured");
            }
        }
        
        void ConfigureSpatialAwareness(ShafinaSpatialAwareness spatialAwareness)
        {
            // Set detection range
            var detectionRangeField = typeof(ShafinaSpatialAwareness).GetField("detectionRange", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (detectionRangeField != null)
            {
                detectionRangeField.SetValue(spatialAwareness, 3f);
            }
            
            // Set detection interval
            var detectionIntervalField = typeof(ShafinaSpatialAwareness).GetField("detectionInterval", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (detectionIntervalField != null)
            {
                detectionIntervalField.SetValue(spatialAwareness, 0.5f);
            }
            
            // Set spatial layer mask
            var spatialLayerMaskField = typeof(ShafinaSpatialAwareness).GetField("spatialLayerMask", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (spatialLayerMaskField != null)
            {
                spatialLayerMaskField.SetValue(spatialAwareness, -1); // All layers
            }
            
            Log("Spatial awareness configured with default settings");
        }
        
        void LoadMaterialsAndPrefabs()
        {
            Log("Loading materials and prefabs...");
            
            // Load spatial indicator prefab
            if (spatialIndicatorPrefab == null)
            {
                spatialIndicatorPrefab = Resources.Load<GameObject>("SpatialIndicator");
                if (spatialIndicatorPrefab == null)
                {
                    LogWarning("SpatialIndicator prefab not found in Resources folder");
                }
            }
            
            // Load materials
            if (wallMaterial == null)
            {
                wallMaterial = Resources.Load<Material>("SpatialWall");
                if (wallMaterial == null)
                {
                    LogWarning("SpatialWall material not found in Resources folder");
                }
            }
            
            if (floorMaterial == null)
            {
                floorMaterial = Resources.Load<Material>("SpatialFloor");
                if (floorMaterial == null)
                {
                    LogWarning("SpatialFloor material not found in Resources folder");
                }
            }
            
            if (furnitureMaterial == null)
            {
                furnitureMaterial = Resources.Load<Material>("SpatialFurniture");
                if (furnitureMaterial == null)
                {
                    LogWarning("SpatialFurniture material not found in Resources folder");
                }
            }
            
            // Assign materials to spatial awareness component
            AssignMaterialsToSpatialAwareness();
        }
        
        void AssignMaterialsToSpatialAwareness()
        {
            ShafinaSpatialAwareness spatialAwareness = FindObjectOfType<ShafinaSpatialAwareness>();
            if (spatialAwareness != null)
            {
                // Use reflection to set private fields
                var wallMaterialField = typeof(ShafinaSpatialAwareness).GetField("wallMaterial", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (wallMaterialField != null && wallMaterial != null)
                {
                    wallMaterialField.SetValue(spatialAwareness, wallMaterial);
                }
                
                var floorMaterialField = typeof(ShafinaSpatialAwareness).GetField("floorMaterial", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (floorMaterialField != null && floorMaterial != null)
                {
                    floorMaterialField.SetValue(spatialAwareness, floorMaterial);
                }
                
                var furnitureMaterialField = typeof(ShafinaSpatialAwareness).GetField("furnitureMaterial", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (furnitureMaterialField != null && furnitureMaterial != null)
                {
                    furnitureMaterialField.SetValue(spatialAwareness, furnitureMaterial);
                }
                
                var spatialIndicatorPrefabField = typeof(ShafinaSpatialAwareness).GetField("spatialIndicatorPrefab", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (spatialIndicatorPrefabField != null && spatialIndicatorPrefab != null)
                {
                    spatialIndicatorPrefabField.SetValue(spatialAwareness, spatialIndicatorPrefab);
                }
                
                Log("Materials and prefabs assigned to spatial awareness component");
            }
        }
        
        [ContextMenu("Test Spatial Awareness")]
        public void TestSpatialAwareness()
        {
            ShafinaSpatialAwareness spatialAwareness = FindObjectOfType<ShafinaSpatialAwareness>();
            if (spatialAwareness != null)
            {
                Log("Testing spatial awareness...");
                
                // Trigger manual reactions for testing
                ShafinaSpatialDialogue spatialDialogue = FindObjectOfType<ShafinaSpatialDialogue>();
                if (spatialDialogue != null)
                {
                    spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Wall);
                    spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Floor);
                    spatialDialogue.TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType.Furniture);
                }
                
                Log("Spatial awareness test completed");
            }
            else
            {
                LogError("No ShafinaSpatialAwareness component found!");
            }
        }
        
        [ContextMenu("Clear All Spatial Data")]
        public void ClearAllSpatialData()
        {
            ShafinaSpatialAwareness spatialAwareness = FindObjectOfType<ShafinaSpatialAwareness>();
            if (spatialAwareness != null)
            {
                spatialAwareness.ClearDetectedObjects();
                Log("All spatial data cleared");
            }
            
            ShafinaSpatialDialogue spatialDialogue = FindObjectOfType<ShafinaSpatialDialogue>();
            if (spatialDialogue != null)
            {
                spatialDialogue.ClearReactionHistory();
                Log("Reaction history cleared");
            }
        }
        
        void Log(string message)
        {
            if (showSetupLogs)
            {
                Debug.Log($"[ShafinaSpatialSetup] {message}");
            }
        }
        
        void LogWarning(string message)
        {
            if (showSetupLogs)
            {
                Debug.LogWarning($"[ShafinaSpatialSetup] {message}");
            }
        }
        
        void LogError(string message)
        {
            if (showSetupLogs)
            {
                Debug.LogError($"[ShafinaSpatialSetup] {message}");
            }
        }
    }
}
