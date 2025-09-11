using UnityEngine;
using UnityEngine.InputSystem;

namespace ConvaiMR
{
    [CreateAssetMenu(fileName = "ConvaiQuestInputReferences", menuName = "Convai/Quest Input References")]
    public class ConvaiQuestInputReferences : ScriptableObject
    {
        [Header("Quest Controller Input Actions")]
        public InputActionReference pushToTalkAction;
        public InputActionReference menuToggleAction;
        public InputActionReference interactAction;
        
        [Header("Input Action Asset")]
        public InputActionAsset questInputActions;
        
        void OnEnable()
        {
            // Enable all input actions when this asset is loaded
            if (questInputActions != null)
            {
                questInputActions.Enable();
            }
        }
        
        void OnDisable()
        {
            // Disable all input actions when this asset is unloaded
            if (questInputActions != null)
            {
                questInputActions.Disable();
            }
        }
    }
}