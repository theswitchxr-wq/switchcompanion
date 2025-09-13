using UnityEngine;
using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;
using System.Collections;

namespace ConvaiMR
{
    /// <summary>
    /// Integrates spatial awareness with Convai's dialogue system
    /// Allows Shafina to react to spatial objects through voice and text
    /// </summary>
    public class ShafinaSpatialDialogue : MonoBehaviour
    {
        [Header("Convai Integration")]
        [SerializeField] private ConvaiNPC convaiNPC;
        [SerializeField] private ShafinaSpatialAwareness spatialAwareness;
        
        [Header("Dialogue Settings")]
        [SerializeField] private float reactionCooldown = 5f;
        [SerializeField] private bool enableVoiceReactions = true;
        [SerializeField] private bool enableTextReactions = true;
        
        [Header("Spatial Reactions")]
        [SerializeField] private SpatialReaction[] spatialReactions;
        
        // Private variables
        private Dictionary<ShafinaSpatialAwareness.SpatialObjectType, float> lastReactionTimes = new Dictionary<ShafinaSpatialAwareness.SpatialObjectType, float>();
        private Coroutine dialogueCoroutine;
        
        [System.Serializable]
        public class SpatialReaction
        {
            public ShafinaSpatialAwareness.SpatialObjectType objectType;
            public string[] reactionTexts;
            public AudioClip[] reactionAudio;
            public float priority = 1f;
        }
        
        void Start()
        {
            InitializeComponents();
            SetupSpatialReactions();
        }
        
        void InitializeComponents()
        {
            // Get Convai NPC component
            if (convaiNPC == null)
            {
                convaiNPC = GetComponent<ConvaiNPC>();
                if (convaiNPC == null)
                {
                    convaiNPC = GetComponentInChildren<ConvaiNPC>();
                }
            }
            
            if (convaiNPC == null)
            {
                Debug.LogError("[ShafinaSpatialDialogue] No ConvaiNPC found!");
                return;
            }
            
            // Get spatial awareness component
            if (spatialAwareness == null)
            {
                spatialAwareness = GetComponent<ShafinaSpatialAwareness>();
                if (spatialAwareness == null)
                {
                    spatialAwareness = GetComponentInChildren<ShafinaSpatialAwareness>();
                }
            }
            
            if (spatialAwareness == null)
            {
                Debug.LogError("[ShafinaSpatialDialogue] No ShafinaSpatialAwareness found!");
                return;
            }
            
            Debug.Log("[ShafinaSpatialDialogue] Spatial dialogue system initialized");
        }
        
        void SetupSpatialReactions()
        {
            // Initialize default reactions if none are set
            if (spatialReactions == null || spatialReactions.Length == 0)
            {
                spatialReactions = new SpatialReaction[]
                {
                    new SpatialReaction
                    {
                        objectType = ShafinaSpatialAwareness.SpatialObjectType.Wall,
                        reactionTexts = new string[]
                        {
                            "I can see a wall here. This helps me understand the room layout!",
                            "There's a wall nearby. I'll remember this for navigation.",
                            "I notice a wall structure. This gives me spatial context."
                        },
                        priority = 1f
                    },
                    new SpatialReaction
                    {
                        objectType = ShafinaSpatialAwareness.SpatialObjectType.Floor,
                        reactionTexts = new string[]
                        {
                            "This is the floor. I can walk around here safely.",
                            "I can see the ground beneath me. This is my walking space.",
                            "The floor is clear here. Perfect for movement."
                        },
                        priority = 0.5f
                    },
                    new SpatialReaction
                    {
                        objectType = ShafinaSpatialAwareness.SpatialObjectType.Ceiling,
                        reactionTexts = new string[]
                        {
                            "I can see the ceiling above me.",
                            "The ceiling is quite high here.",
                            "I notice the overhead structure."
                        },
                        priority = 0.3f
                    },
                    new SpatialReaction
                    {
                        objectType = ShafinaSpatialAwareness.SpatialObjectType.Furniture,
                        reactionTexts = new string[]
                        {
                            "I notice some furniture nearby. I'll be careful around it.",
                            "There's furniture here. I'll avoid bumping into it.",
                            "I can see some objects in the room. I'll navigate around them."
                        },
                        priority = 0.8f
                    }
                };
            }
            
            // Initialize reaction times
            foreach (var reaction in spatialReactions)
            {
                if (!lastReactionTimes.ContainsKey(reaction.objectType))
                {
                    lastReactionTimes[reaction.objectType] = 0f;
                }
            }
        }
        
        void Update()
        {
            if (spatialAwareness != null && convaiNPC != null)
            {
                CheckForSpatialReactions();
            }
        }
        
        void CheckForSpatialReactions()
        {
            var detectedObjects = spatialAwareness.GetDetectedObjects();
            
            foreach (var spatialObj in detectedObjects)
            {
                if (spatialObj.hasReacted) continue;
                
                // Check cooldown
                if (Time.time - lastReactionTimes[spatialObj.type] < reactionCooldown)
                    continue;
                
                // Find appropriate reaction
                SpatialReaction reaction = GetReactionForType(spatialObj.type);
                if (reaction != null)
                {
                    TriggerSpatialReaction(spatialObj, reaction);
                    lastReactionTimes[spatialObj.type] = Time.time;
                }
            }
        }
        
        SpatialReaction GetReactionForType(ShafinaSpatialAwareness.SpatialObjectType objectType)
        {
            foreach (var reaction in spatialReactions)
            {
                if (reaction.objectType == objectType)
                {
                    return reaction;
                }
            }
            return null;
        }
        
        void TriggerSpatialReaction(ShafinaSpatialAwareness.SpatialObject spatialObj, SpatialReaction reaction)
        {
            if (reaction.reactionTexts == null || reaction.reactionTexts.Length == 0)
                return;
            
            // Select random reaction text
            string reactionText = reaction.reactionTexts[Random.Range(0, reaction.reactionTexts.Length)];
            
            Debug.Log($"[ShafinaSpatialDialogue] Shafina reacts to {spatialObj.type}: {reactionText}");
            
            // Trigger Convai dialogue
            if (enableVoiceReactions && convaiNPC != null)
            {
                // Note: This would need to be integrated with Convai's actual dialogue system
                // For now, we'll just log the reaction
                StartCoroutine(SimulateDialogueReaction(reactionText));
            }
            
            if (enableTextReactions)
            {
                // Display text reaction (could be UI text, subtitle, etc.)
                DisplayTextReaction(reactionText);
            }
            
            // Mark as reacted
            spatialObj.hasReacted = true;
        }
        
        IEnumerator SimulateDialogueReaction(string text)
        {
            // This is a placeholder for actual Convai integration
            // In a real implementation, you would use Convai's dialogue API here
            
            Debug.Log($"[ShafinaSpatialDialogue] [VOICE] Shafina: {text}");
            
            // Simulate dialogue duration
            yield return new WaitForSeconds(text.Length * 0.1f);
        }
        
        void DisplayTextReaction(string text)
        {
            // This could display text on UI, subtitles, or debug console
            Debug.Log($"[ShafinaSpatialDialogue] [TEXT] {text}");
            
            // TODO: Implement UI text display
            // Example: subtitleUI.ShowText(text, 3f);
        }
        
        // Public methods for external control
        public void SetReactionEnabled(ShafinaSpatialAwareness.SpatialObjectType objectType, bool enabled)
        {
            var reaction = GetReactionForType(objectType);
            if (reaction != null)
            {
                // This would need to be implemented based on your reaction system
                Debug.Log($"[ShafinaSpatialDialogue] {objectType} reactions: {(enabled ? "ENABLED" : "DISABLED")}");
            }
        }
        
        public void SetReactionCooldown(float cooldown)
        {
            reactionCooldown = cooldown;
            Debug.Log($"[ShafinaSpatialDialogue] Reaction cooldown set to {cooldown} seconds");
        }
        
        public void TriggerManualReaction(ShafinaSpatialAwareness.SpatialObjectType objectType)
        {
            var reaction = GetReactionForType(objectType);
            if (reaction != null)
            {
                // Create a dummy spatial object for manual reaction
                var dummySpatialObj = new ShafinaSpatialAwareness.SpatialObject(
                    Vector3.zero, Vector3.up, objectType, 1f);
                
                TriggerSpatialReaction(dummySpatialObj, reaction);
            }
        }
        
        public void ClearReactionHistory()
        {
            var keys = new List<ShafinaSpatialAwareness.SpatialObjectType>(lastReactionTimes.Keys);
            foreach (var key in keys)
            {
                lastReactionTimes[key] = 0f;
            }
            Debug.Log("[ShafinaSpatialDialogue] Reaction history cleared");
        }
    }
}
