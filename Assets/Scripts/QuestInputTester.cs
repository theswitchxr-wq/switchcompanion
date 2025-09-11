using UnityEngine;
using UnityEngine.InputSystem;

namespace ConvaiMR
{
    /// <summary>
    /// Simple test script to verify Quest controller input is working
    /// </summary>
    public class QuestInputTester : MonoBehaviour
    {
        [Header("Debug")]
        public bool showDebugInfo = true;
        public KeyCode debugKey = KeyCode.F1;
        
        private InputAction _talkAction;
        private bool _isGripPressed = false;
        
        void Start()
        {
            // Load the input actions asset
            var inputActions = Resources.Load<InputActionAsset>("Controls");
            if (inputActions != null)
            {
                _talkAction = inputActions.FindAction("Talk");
                if (_talkAction != null)
                {
                    _talkAction.Enable();
                    Debug.Log("[QuestInputTester] Talk action enabled successfully!");
                }
                else
                {
                    Debug.LogError("[QuestInputTester] Talk action not found!");
                }
            }
            else
            {
                Debug.LogError("[QuestInputTester] Controls.inputactions not found!");
            }
        }
        
        void Update()
        {
            // Toggle debug info with F1 key
            if (Input.GetKeyDown(debugKey))
            {
                showDebugInfo = !showDebugInfo;
            }
            
            // Check for grip button input
            if (_talkAction != null)
            {
                bool wasPressed = _talkAction.WasPressedThisFrame();
                bool wasReleased = _talkAction.WasReleasedThisFrame();
                bool isPressed = _talkAction.IsPressed();
                
                if (wasPressed)
                {
                    Debug.Log("[QuestInputTester] Grip button PRESSED!");
                    _isGripPressed = true;
                }
                
                if (wasReleased)
                {
                    Debug.Log("[QuestInputTester] Grip button RELEASED!");
                    _isGripPressed = false;
                }
            }
        }
        
        void OnGUI()
        {
            if (!showDebugInfo) return;
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("Quest Input Tester", GUI.skin.box);
            GUILayout.Label($"Grip Button: {(_isGripPressed ? "PRESSED" : "Released")}");
            GUILayout.Label($"Talk Action Enabled: {(_talkAction?.enabled ?? false)}");
            GUILayout.Label($"Press {debugKey} to toggle this info");
            GUILayout.EndArea();
        }
        
        void OnDestroy()
        {
            if (_talkAction != null)
            {
                _talkAction.Disable();
            }
        }
    }
}
