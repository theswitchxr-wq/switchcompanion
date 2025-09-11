using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

namespace ConvaiMR
{
    public class ConvaiAvatarPlacer : MonoBehaviour
    {
        [Header("Avatar Settings")]
        public GameObject convaiAvatarPrefab;
        public LayerMask placementLayerMask = 1;
        
        [Header("Placement Feedback")]
        public GameObject placementIndicator;
        
        private Camera arCamera;
        private GameObject currentAvatar;
        private bool isAvatarPlaced = false;
        
        // Input Actions
        private InputAction tapAction;
        
        void Start()
        {
            arCamera = Camera.main;
            
            // Setup input for Quest hand tracking / controllers
            tapAction = new InputAction(binding: "<XRController>/trigger");
            tapAction.Enable();
            tapAction.performed += OnTapPerformed;
            
            // If no prefab assigned, find existing avatar
            if (convaiAvatarPrefab == null)
            {
                var existingAvatar = FindObjectOfType<Convai.Scripts.Runtime.Core.ConvaiNPC>();
                if (existingAvatar != null)
                    convaiAvatarPrefab = existingAvatar.gameObject;
            }
        }
        
        void Update()
        {
            if (!isAvatarPlaced)
                UpdatePlacementIndicator();
        }
        
        void UpdatePlacementIndicator()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = arCamera.ScreenPointToRay(screenCenter);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, placementLayerMask))
            {
                if (placementIndicator != null)
                {
                    placementIndicator.SetActive(true);
                    placementIndicator.transform.position = hit.point;
                    placementIndicator.transform.LookAt(hit.point + hit.normal);
                }
            }
            else if (placementIndicator != null)
            {
                placementIndicator.SetActive(false);
            }
        }
        
        void OnTapPerformed(InputAction.CallbackContext context)
        {
            if (isAvatarPlaced) return;
            
            PlaceAvatar();
        }
        
        public void PlaceAvatar()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = arCamera.ScreenPointToRay(screenCenter);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, placementLayerMask))
            {
                if (convaiAvatarPrefab != null)
                {
                    // Place avatar at hit point
                    if (currentAvatar != null)
                        DestroyImmediate(currentAvatar);
                        
                    currentAvatar = Instantiate(convaiAvatarPrefab, hit.point, Quaternion.identity);
                    
                    // Make avatar face the user
                    Vector3 lookDirection = (arCamera.transform.position - hit.point).normalized;
                    lookDirection.y = 0; // Keep avatar upright
                    currentAvatar.transform.rotation = Quaternion.LookRotation(lookDirection);
                    
                    isAvatarPlaced = true;
                    
                    if (placementIndicator != null)
                        placementIndicator.SetActive(false);
                        
                    Debug.Log($"Convai avatar placed at {hit.point}");
                }
            }
        }
        
        public void ResetPlacement()
        {
            isAvatarPlaced = false;
            if (currentAvatar != null)
            {
                DestroyImmediate(currentAvatar);
                currentAvatar = null;
            }
        }
        
        void OnDestroy()
        {
            tapAction?.Dispose();
        }
    }
}