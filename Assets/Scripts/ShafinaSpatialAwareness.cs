using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;
using System.Collections;

namespace ConvaiMR
{
    /// <summary>
    /// Provides spatial awareness capabilities to Shafina (Convai avatar) in Mixed Reality
    /// Detects walls, floors, furniture and enables contextual interactions
    /// </summary>
    public class ShafinaSpatialAwareness : MonoBehaviour
    {
        [Header("Spatial Detection Settings")]
        [SerializeField] private float detectionRange = 3f;
        [SerializeField] private float detectionInterval = 0.5f;
        [SerializeField] private LayerMask spatialLayerMask = -1;
        
        [Header("Visual Feedback")]
        [SerializeField] private GameObject spatialIndicatorPrefab;
        [SerializeField] private Material wallMaterial;
        [SerializeField] private Material floorMaterial;
        [SerializeField] private Material furnitureMaterial;
        
        [Header("Audio Feedback")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] spatialReactionClips;
        
        [Header("Debug")]
        [SerializeField] private bool showDebugRays = true;
        [SerializeField] private bool enableSpatialReactions = true;
        
        // Private variables
        private ConvaiNPC convaiNPC;
        private ARPlaneManager arPlaneManager;
        private ARRaycastManager arRaycastManager;
        private Camera arCamera;
        private Transform avatarTransform;
        private UnityEngine.XR.Interaction.Toolkit.XROrigin xrOrigin;
        
        // Spatial awareness data
        private Dictionary<Vector3, SpatialObject> detectedObjects = new Dictionary<Vector3, SpatialObject>();
        private List<GameObject> spatialIndicators = new List<GameObject>();
        private Coroutine detectionCoroutine;
        
        // Spatial object types
        public enum SpatialObjectType
        {
            Wall,
            Floor,
            Ceiling,
            Furniture,
            Unknown
        }
        
        [System.Serializable]
        public class SpatialObject
        {
            public Vector3 position;
            public Vector3 normal;
            public SpatialObjectType type;
            public float confidence;
            public GameObject indicator;
            public bool hasReacted;
            
            public SpatialObject(Vector3 pos, Vector3 norm, SpatialObjectType objType, float conf)
            {
                position = pos;
                normal = norm;
                type = objType;
                confidence = conf;
                hasReacted = false;
            }
        }
        
        void Start()
        {
            InitializeComponents();
            StartSpatialDetection();
        }
        
        void InitializeComponents()
        {
            // Get Convai NPC component
            convaiNPC = GetComponent<ConvaiNPC>();
            if (convaiNPC == null)
            {
                convaiNPC = GetComponentInChildren<ConvaiNPC>();
            }
            
            if (convaiNPC == null)
            {
                Debug.LogError("[ShafinaSpatialAwareness] No ConvaiNPC found! This script should be attached to the avatar.");
                return;
            }
            
            avatarTransform = transform;
            
            // Get AR Foundation components
            arPlaneManager = FindObjectOfType<ARPlaneManager>();
            arRaycastManager = FindObjectOfType<ARRaycastManager>();
            xrOrigin = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.XROrigin>();
            arCamera = Camera.main;
            
            if (arPlaneManager == null)
            {
                Debug.LogWarning("[ShafinaSpatialAwareness] ARPlaneManager not found. Some spatial features may not work.");
            }
            
            if (arRaycastManager == null)
            {
                Debug.LogWarning("[ShafinaSpatialAwareness] ARRaycastManager not found. Raycast-based detection disabled.");
            }
            
            // Setup audio source if not assigned
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1f; // 3D audio
            }
            
            Debug.Log("[ShafinaSpatialAwareness] Spatial awareness initialized for " + convaiNPC.name);
        }
        
        void StartSpatialDetection()
        {
            if (detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
            }
            
            detectionCoroutine = StartCoroutine(SpatialDetectionLoop());
        }
        
        IEnumerator SpatialDetectionLoop()
        {
            while (true)
            {
                DetectNearbySurfaces();
                yield return new WaitForSeconds(detectionInterval);
            }
        }
        
        void DetectNearbySurfaces()
        {
            if (avatarTransform == null) return;
            
            // Cast rays in multiple directions from the avatar
            Vector3[] rayDirections = {
                Vector3.forward,
                Vector3.back,
                Vector3.left,
                Vector3.right,
                Vector3.down, // Floor detection
                Vector3.up    // Ceiling detection
            };
            
            foreach (Vector3 direction in rayDirections)
            {
                Ray ray = new Ray(avatarTransform.position, direction);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, detectionRange, spatialLayerMask))
                {
                    ProcessSpatialHit(hit);
                }
                
                // Debug visualization
                if (showDebugRays)
                {
                    Debug.DrawRay(ray.origin, ray.direction * detectionRange, Color.cyan, detectionInterval);
                }
            }
            
            // Also use AR Foundation plane detection
            if (arPlaneManager != null)
            {
                DetectARPlanes();
            }
        }
        
        void ProcessSpatialHit(RaycastHit hit)
        {
            Vector3 roundedPos = RoundToGrid(hit.point, 0.1f);
            
            // Avoid duplicate detections
            if (detectedObjects.ContainsKey(roundedPos))
            {
                return;
            }
            
            // Determine object type based on normal and position
            SpatialObjectType objectType = ClassifySpatialObject(hit);
            
            // Create spatial object
            SpatialObject spatialObj = new SpatialObject(
                hit.point,
                hit.normal,
                objectType,
                CalculateConfidence(hit)
            );
            
            detectedObjects[roundedPos] = spatialObj;
            
            // Create visual indicator
            CreateSpatialIndicator(spatialObj);
            
            // Trigger reaction if enabled
            if (enableSpatialReactions)
            {
                TriggerSpatialReaction(spatialObj);
            }
            
            Debug.Log($"[ShafinaSpatialAwareness] Detected {objectType} at {hit.point} (confidence: {spatialObj.confidence:F2})");
        }
        
        SpatialObjectType ClassifySpatialObject(RaycastHit hit)
        {
            Vector3 normal = hit.normal;
            float yComponent = Mathf.Abs(normal.y);
            
            // Floor detection (normal pointing up)
            if (normal.y > 0.7f)
            {
                return SpatialObjectType.Floor;
            }
            // Ceiling detection (normal pointing down)
            else if (normal.y < -0.7f)
            {
                return SpatialObjectType.Ceiling;
            }
            // Wall detection (normal mostly horizontal)
            else if (yComponent < 0.3f)
            {
                return SpatialObjectType.Wall;
            }
            // Furniture or other objects
            else
            {
                return SpatialObjectType.Furniture;
            }
        }
        
        float CalculateConfidence(RaycastHit hit)
        {
            // Base confidence on distance and normal alignment
            float distanceFactor = 1f - (hit.distance / detectionRange);
            float normalFactor = Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up));
            
            return Mathf.Clamp01(distanceFactor * 0.7f + normalFactor * 0.3f);
        }
        
        void DetectARPlanes()
        {
            if (arPlaneManager == null) return;
            
            foreach (var plane in arPlaneManager.trackables)
            {
                if (plane.trackingState == TrackingState.Tracking)
                {
                    // Check if plane is near the avatar
                    float distance = Vector3.Distance(avatarTransform.position, plane.transform.position);
                    if (distance <= detectionRange)
                    {
                        // Create spatial object for AR plane
                        Vector3 roundedPos = RoundToGrid(plane.transform.position, 0.1f);
                        
                        if (!detectedObjects.ContainsKey(roundedPos))
                        {
                            SpatialObjectType planeType = plane.alignment == PlaneAlignment.HorizontalUp ? 
                                SpatialObjectType.Floor : SpatialObjectType.Wall;
                            
                            SpatialObject spatialObj = new SpatialObject(
                                plane.transform.position,
                                plane.normal,
                                planeType,
                                0.9f // High confidence for AR planes
                            );
                            
                            detectedObjects[roundedPos] = spatialObj;
                            CreateSpatialIndicator(spatialObj);
                            
                            Debug.Log($"[ShafinaSpatialAwareness] Detected AR {planeType} plane");
                        }
                    }
                }
            }
        }
        
        void CreateSpatialIndicator(SpatialObject spatialObj)
        {
            if (spatialIndicatorPrefab == null) return;
            
            GameObject indicator = Instantiate(spatialIndicatorPrefab, spatialObj.position, Quaternion.LookRotation(spatialObj.normal));
            spatialObj.indicator = indicator;
            spatialIndicators.Add(indicator);
            
            // Apply appropriate material based on object type
            Renderer renderer = indicator.GetComponent<Renderer>();
            if (renderer != null)
            {
                switch (spatialObj.type)
                {
                    case SpatialObjectType.Wall:
                        if (wallMaterial != null) renderer.material = wallMaterial;
                        break;
                    case SpatialObjectType.Floor:
                        if (floorMaterial != null) renderer.material = floorMaterial;
                        break;
                    case SpatialObjectType.Furniture:
                        if (furnitureMaterial != null) renderer.material = furnitureMaterial;
                        break;
                }
            }
            
            // Scale based on confidence
            float scale = 0.5f + (spatialObj.confidence * 0.5f);
            indicator.transform.localScale = Vector3.one * scale;
        }
        
        void TriggerSpatialReaction(SpatialObject spatialObj)
        {
            if (spatialObj.hasReacted) return;
            
            spatialObj.hasReacted = true;
            
            // Play audio reaction
            if (audioSource != null && spatialReactionClips != null && spatialReactionClips.Length > 0)
            {
                AudioClip clip = spatialReactionClips[Random.Range(0, spatialReactionClips.Length)];
                audioSource.PlayOneShot(clip);
            }
            
            // Trigger Convai dialogue based on detected object
            if (convaiNPC != null)
            {
                string reactionText = GetSpatialReactionText(spatialObj);
                if (!string.IsNullOrEmpty(reactionText))
                {
                    // Note: This would need to be integrated with Convai's dialogue system
                    Debug.Log($"[ShafinaSpatialAwareness] Shafina says: {reactionText}");
                }
            }
        }
        
        string GetSpatialReactionText(SpatialObject spatialObj)
        {
            switch (spatialObj.type)
            {
                case SpatialObjectType.Wall:
                    return "I can see a wall here. This helps me understand the room layout!";
                case SpatialObjectType.Floor:
                    return "This is the floor. I can walk around here safely.";
                case SpatialObjectType.Ceiling:
                    return "I can see the ceiling above me.";
                case SpatialObjectType.Furniture:
                    return "I notice some furniture nearby. I'll be careful around it.";
                default:
                    return "I can sense something in the space around me.";
            }
        }
        
        Vector3 RoundToGrid(Vector3 position, float gridSize)
        {
            return new Vector3(
                Mathf.Round(position.x / gridSize) * gridSize,
                Mathf.Round(position.y / gridSize) * gridSize,
                Mathf.Round(position.z / gridSize) * gridSize
            );
        }
        
        // Public methods for external access
        public List<SpatialObject> GetDetectedObjects()
        {
            return new List<SpatialObject>(detectedObjects.Values);
        }
        
        public List<SpatialObject> GetObjectsOfType(SpatialObjectType type)
        {
            List<SpatialObject> objects = new List<SpatialObject>();
            foreach (var obj in detectedObjects.Values)
            {
                if (obj.type == type)
                {
                    objects.Add(obj);
                }
            }
            return objects;
        }
        
        public void ClearDetectedObjects()
        {
            foreach (var indicator in spatialIndicators)
            {
                if (indicator != null)
                {
                    DestroyImmediate(indicator);
                }
            }
            spatialIndicators.Clear();
            detectedObjects.Clear();
        }
        
        void OnDestroy()
        {
            if (detectionCoroutine != null)
            {
                StopCoroutine(detectionCoroutine);
            }
            
            ClearDetectedObjects();
        }
        
        void OnDrawGizmos()
        {
            if (showDebugRays && avatarTransform != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(avatarTransform.position, detectionRange);
            }
        }
    }
}
