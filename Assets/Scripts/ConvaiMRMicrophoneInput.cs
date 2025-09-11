using UnityEngine;
using UnityEngine.InputSystem;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    public class ConvaiMRMicrophoneInput : MonoBehaviour
    {
        [Header("Convai Integration")]
        [Tooltip("Reference to the Convai NPC to interact with")]
        public ConvaiNPC targetNPC;
        
        [Header("Input Controls")]
        [Tooltip("Enable PC testing mode (T key for push-to-talk)")]
        public bool enablePCTesting = true;
        [Tooltip("Enable Quest controller support (grip button for push-to-talk)")]
        public bool enableQuestSupport = true;
        [Tooltip("PC Key for Push-to-Talk (default: T)")]
        public Key pcPushToTalkKey = Key.T;
        
        [Header("Visual Feedback")]
        public GameObject recordingIndicator;
        [Tooltip("UI Text to show controls (optional)")]
        public UnityEngine.UI.Text controlsText;
        
        private ConvaiInputManager _inputManager;
        private InputAction _talkAction;
        private bool _isListening = false;
        
        void Start()
        {
            Debug.Log("[ConvaiMR] Microphone input starting...");
            
            // Find the ConvaiInputManager
            _inputManager = FindObjectOfType<ConvaiInputManager>();
            if (_inputManager == null)
            {
                Debug.LogError("[ConvaiMR] ConvaiInputManager not found! Please add it to the scene.");
                return;
            }
            
            // Find the target NPC if not assigned
            if (targetNPC == null)
            {
                targetNPC = FindObjectOfType<ConvaiNPC>();
                if (targetNPC == null)
                {
                    Debug.LogError("[ConvaiMR] No ConvaiNPC found! Please add one to the scene.");
                    return;
                }
            }
            
            // Initialize input actions
            InitializeInputActions();
            
            // Subscribe to the talk key events
            _inputManager.talkKeyInteract += OnTalkKeyInteract;
            
            Debug.Log("[ConvaiMR] Microphone input initialized successfully!");
            Debug.Log($"[ConvaiMR] Target NPC: {targetNPC.characterName}");
            Debug.Log($"[ConvaiMR] PC Testing: {(enablePCTesting ? "Enabled (T key)" : "Disabled")}");
            Debug.Log($"[ConvaiMR] Quest Support: {(enableQuestSupport ? "Enabled (Grip button)" : "Disabled")}");
        }
        
        void InitializeInputActions()
        {
            // Load the input actions asset
            var inputActions = Resources.Load<InputActionAsset>("Controls");
            if (inputActions == null)
            {
                Debug.LogError("[ConvaiMR] Controls.inputactions not found in Resources folder!");
                return;
            }
            
            // Get the Talk action
            _talkAction = inputActions.FindAction("Talk");
            if (_talkAction == null)
            {
                Debug.LogError("[ConvaiMR] Talk action not found in Controls.inputactions!");
                return;
            }
            
            // Enable the action
            _talkAction.Enable();
            
            Debug.Log("[ConvaiMR] Input actions initialized successfully!");
        }
        
        void Update()
        {
            // Handle input using the unified Talk action (works for both PC and Quest)
            if (_talkAction != null)
            {
                if (_talkAction.WasPressedThisFrame())
                {
                    OnTalkKeyInteract(true);
                }
                else if (_talkAction.WasReleasedThisFrame())
                {
                    OnTalkKeyInteract(false);
                }
            }
            
            // Fallback: Handle PC testing input using direct keyboard input
            if (enablePCTesting && Keyboard.current != null)
            {
                if (Keyboard.current[pcPushToTalkKey].wasPressedThisFrame)
                {
                    OnTalkKeyInteract(true);
                }
                else if (Keyboard.current[pcPushToTalkKey].wasReleasedThisFrame)
                {
                    OnTalkKeyInteract(false);
                }
            }
            
            // Update visual feedback
            UpdateVisualFeedback();
        }
        
        void OnTalkKeyInteract(bool isPressed)
        {
            if (targetNPC == null) return;
            
            if (isPressed && !_isListening)
            {
                // Start listening
                targetNPC.StartListening();
                _isListening = true;
                Debug.Log("[ConvaiMR] Started listening to NPC");
            }
            else if (!isPressed && _isListening)
            {
                // Stop listening
                targetNPC.StopListening();
                _isListening = false;
                Debug.Log("[ConvaiMR] Stopped listening to NPC");
            }
        }
        
        void UpdateVisualFeedback()
        {
            if (recordingIndicator != null)
            {
                recordingIndicator.SetActive(_isListening);
            }
            
            if (controlsText != null)
            {
                string status = _isListening ? "LISTENING" : "Ready";
                string controls = "";
                
                if (enablePCTesting && enableQuestSupport)
                {
                    controls = "T Key (PC) / Grip Button (Quest)";
                }
                else if (enablePCTesting)
                {
                    controls = "T Key (PC)";
                }
                else if (enableQuestSupport)
                {
                    controls = "Grip Button (Quest)";
                }
                else
                {
                    controls = "None";
                }
                
                controlsText.text = $"Convai MR Controls\n" +
                                  $"Push-to-Talk: {controls}\n" +
                                  $"Status: {status}\n" +
                                  $"NPC: {targetNPC?.characterName ?? "None"}";
            }
        }
        
        void OnDestroy()
        {
            // Unsubscribe from events
            if (_inputManager != null)
            {
                _inputManager.talkKeyInteract -= OnTalkKeyInteract;
            }
            
            // Disable input action
            if (_talkAction != null)
            {
                _talkAction.Disable();
            }
        }
    }
}