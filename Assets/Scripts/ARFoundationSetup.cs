using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace ConvaiMR
{
    /// <summary>
    /// Sets up AR Foundation components for spatial awareness in Mixed Reality
    /// </summary>
    public class ARFoundationSetup : MonoBehaviour
    {
        [Header("AR Foundation Components")]
        [SerializeField] private ARSession arSession;
        [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private ARPlaneManager arPlaneManager;
        [SerializeField] private ARRaycastManager arRaycastManager;
        [SerializeField] private ARPlaneMeshVisualizer planeMeshVisualizer;
        
        [Header("Plane Detection Settings")]
        [SerializeField] private Material planeMaterial;
        [SerializeField] private Material lineMaterial;
        
        [Header("Debug")]
        [SerializeField] private bool enableDebugVisualization = true;
        
        void Start()
        {
            SetupARFoundation();
        }
        
        void SetupARFoundation()
        {
            Debug.Log("[ARFoundationSetup] Setting up AR Foundation components...");
            
            // Setup AR Session
            if (arSession == null)
            {
                arSession = FindObjectOfType<ARSession>();
                if (arSession == null)
                {
                    GameObject sessionGO = new GameObject("AR Session");
                    arSession = sessionGO.AddComponent<ARSession>();
                }
            }
            
            // Setup XR Origin
            if (xrOrigin == null)
            {
                xrOrigin = FindObjectOfType<XROrigin>();
                if (xrOrigin == null)
                {
                    GameObject originGO = new GameObject("XR Origin");
                    xrOrigin = originGO.AddComponent<XROrigin>();
                    
                    // Add AR Camera
                    Camera arCamera = originGO.GetComponent<Camera>();
                    if (arCamera == null)
                    {
                        arCamera = originGO.AddComponent<Camera>();
                    }
                    arCamera.tag = "MainCamera";
                }
            }
            
            // Setup AR Plane Manager
            if (arPlaneManager == null)
            {
                arPlaneManager = FindObjectOfType<ARPlaneManager>();
                if (arPlaneManager == null)
                {
                    arPlaneManager = xrOrigin.gameObject.AddComponent<ARPlaneManager>();
                }
            }
            
            // Configure plane detection
            if (arPlaneManager != null)
            {
                arPlaneManager.requestedDetectionMode = PlaneDetectionMode.HorizontalAndVertical;
                arPlaneManager.detectionMode = PlaneDetectionMode.HorizontalAndVertical;
                
                // Set plane prefab
                if (planeMeshVisualizer == null)
                {
                    // Try to find the ARPlane prefab
                    GameObject planePrefab = Resources.Load<GameObject>("ARPlane");
                    if (planePrefab == null)
                    {
                        // Create a simple plane prefab
                        planePrefab = CreatePlanePrefab();
                    }
                    
                    if (planePrefab != null)
                    {
                        arPlaneManager.planePrefab = planePrefab;
                    }
                }
            }
            
            // Setup AR Raycast Manager
            if (arRaycastManager == null)
            {
                arRaycastManager = FindObjectOfType<ARRaycastManager>();
                if (arRaycastManager == null)
                {
                    arRaycastManager = xrOrigin.gameObject.AddComponent<ARRaycastManager>();
                }
            }
            
            // Configure plane visualization materials
            if (planeMaterial != null && lineMaterial != null)
            {
                Debug.Log("[ARFoundationSetup] Plane materials configured for visualization");
            }
            
            Debug.Log("[ARFoundationSetup] AR Foundation setup completed!");
        }
        
        GameObject CreatePlanePrefab()
        {
            GameObject planePrefab = new GameObject("ARPlane");
            
            // Add MeshFilter
            MeshFilter meshFilter = planePrefab.AddComponent<MeshFilter>();
            meshFilter.mesh = CreatePlaneMesh();
            
            // Add MeshRenderer
            MeshRenderer meshRenderer = planePrefab.AddComponent<MeshRenderer>();
            if (planeMaterial != null)
            {
                meshRenderer.material = planeMaterial;
            }
            else
            {
                // Create a default material
                Material defaultMaterial = new Material(Shader.Find("Standard"));
                defaultMaterial.color = new Color(0, 1, 0, 0.3f);
                defaultMaterial.SetFloat("_Mode", 3); // Transparent
                defaultMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                defaultMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                defaultMaterial.SetInt("_ZWrite", 0);
                defaultMaterial.DisableKeyword("_ALPHATEST_ON");
                defaultMaterial.EnableKeyword("_ALPHABLEND_ON");
                defaultMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                defaultMaterial.renderQueue = 3000;
                meshRenderer.material = defaultMaterial;
            }
            
            // Add ARPlaneMeshVisualizer
            ARPlaneMeshVisualizer visualizer = planePrefab.AddComponent<ARPlaneMeshVisualizer>();
            
            // Add ARPlane
            ARPlane arPlane = planePrefab.AddComponent<ARPlane>();
            
            return planePrefab;
        }
        
        Mesh CreatePlaneMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = "ARPlaneMesh";
            
            // Create a simple quad mesh
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(-0.5f, 0, 0.5f)
            };
            
            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            
            int[] triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3
            };
            
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            
            return mesh;
        }
        
        public void TogglePlaneVisualization()
        {
            if (arPlaneManager != null)
            {
                arPlaneManager.enabled = !arPlaneManager.enabled;
                Debug.Log($"[ARFoundationSetup] Plane detection: {(arPlaneManager.enabled ? "ON" : "OFF")}");
            }
        }
        
        public void ClearAllPlanes()
        {
            if (arPlaneManager != null)
            {
                foreach (var plane in arPlaneManager.trackables)
                {
                    if (plane.gameObject != null)
                    {
                        DestroyImmediate(plane.gameObject);
                    }
                }
                Debug.Log("[ARFoundationSetup] All planes cleared");
            }
        }
    }
}
