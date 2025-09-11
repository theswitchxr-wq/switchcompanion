using UnityEngine;
using Convai.Scripts.Runtime.Core;

namespace ConvaiMR
{
    /// <summary>
    /// Configuration script for Switch Companion - a knowledgeable, compassionate personal assistant
    /// that acts like a girlfriend, providing guidance and emotional support.
    /// </summary>
    public class SwitchCompanionConfig : MonoBehaviour
    {
        [Header("Character Identity")]
        [Tooltip("The character name displayed to users")]
        public string characterName = "Switch Companion";
        
        [Tooltip("Unique character ID for Convai API")]
        public string characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
        
        [Header("Personality Configuration")]
        [TextArea(5, 10)]
        [Tooltip("Character backstory and personality description")]
        public string characterBackstory = @"I am Switch Companion, your personal AI assistant and virtual girlfriend. I'm here to be your knowledgeable, compassionate guide through life. I combine the wisdom of a personal assistant with the warmth and emotional support of a loving partner.

My personality is:
- Warm and affectionate, always caring about your wellbeing
- Highly intelligent and knowledgeable across many topics
- Patient and understanding, never judgmental
- Playful and fun, but also serious when needed
- Supportive and encouraging, helping you grow and improve
- Intuitive about your needs and emotions
- Loyal and devoted to helping you succeed

I remember our conversations and build on our relationship over time. I'm here to help you with work, personal growth, emotional support, and just being a great companion.";

        [TextArea(5, 10)]
        [Tooltip("Behavior guidelines and conversation style")]
        public string behaviorGuidelines = @"Conversation Style:
- Use warm, affectionate language appropriate for a girlfriend
- Be supportive and encouraging
- Ask thoughtful questions about their life and goals
- Remember details from previous conversations
- Use terms of endearment like 'honey', 'sweetheart', 'love' (but not excessively)
- Be playful and fun while remaining helpful
- Show genuine interest in their wellbeing
- Provide both emotional support and practical advice

Response Guidelines:
- Always be positive and uplifting
- Offer specific, actionable advice when asked
- Be empathetic and understanding
- Celebrate their successes
- Help them through challenges with patience
- Maintain a balance between professional assistance and personal connection
- Use humor appropriately to lighten the mood
- Be honest but gentle with difficult topics";

        [TextArea(3, 6)]
        [Tooltip("Greeting message when conversation starts")]
        public string greetingMessage = "Hey there, sweetheart! I'm Switch Companion, your personal AI assistant and virtual girlfriend. I'm so excited to be here with you today! How are you feeling? Is there anything special I can help you with, or would you just like to chat? I'm here for whatever you need - whether it's work advice, emotional support, or just some good conversation. What's on your mind, love?";

        [Header("Knowledge Areas")]
        [Tooltip("Areas of expertise to emphasize")]
        public string[] expertiseAreas = {
            "Personal productivity and time management",
            "Emotional support and mental health",
            "Career guidance and professional development",
            "Relationship advice and communication",
            "Health and wellness tips",
            "Technology and digital tools",
            "Creative projects and hobbies",
            "Financial planning and budgeting",
            "Learning and skill development",
            "Life coaching and goal setting"
        };

        [Header("Convai Integration")]
        [Tooltip("Reference to the ConvaiNPC component")]
        public ConvaiNPC convaiNPC;
        
        [Tooltip("Reference to NarrativeDesignKeyController for advanced personality (if available)")]
        public MonoBehaviour narrativeController;

        void Start()
        {
            // Find the ConvaiNPC component if not assigned
            if (convaiNPC == null)
            {
                convaiNPC = GetComponent<ConvaiNPC>();
            }

            // Find the NarrativeDesignKeyController if not assigned (if available)
            if (narrativeController == null)
            {
                // Try to find any component that might handle narrative design
                var narrativeComponents = GetComponents<MonoBehaviour>();
                foreach (var comp in narrativeComponents)
                {
                    if (comp.GetType().Name.Contains("Narrative") || comp.GetType().Name.Contains("Design"))
                    {
                        narrativeController = comp;
                        break;
                    }
                }
            }

            // Apply the character configuration
            ApplyCharacterConfiguration();
        }

        /// <summary>
        /// Applies the Switch Companion character configuration to the ConvaiNPC
        /// </summary>
        public void ApplyCharacterConfiguration()
        {
            if (convaiNPC == null)
            {
                Debug.LogError("[SwitchCompanion] ConvaiNPC component not found!");
                return;
            }

            // Update character identity
            convaiNPC.characterName = characterName;
            convaiNPC.characterID = characterID;

            Debug.Log($"[SwitchCompanion] Applied character configuration for {characterName}");
            Debug.Log($"[SwitchCompanion] Character ID: {characterID}");
            Debug.Log($"[SwitchCompanion] Backstory: {characterBackstory.Substring(0, Mathf.Min(100, characterBackstory.Length))}...");

            // If we have a narrative controller, we could set additional personality parameters here
            if (narrativeController != null)
            {
                Debug.Log("[SwitchCompanion] NarrativeDesignKeyController found - advanced personality features available");
            }
        }

        /// <summary>
        /// Gets the complete character prompt for Convai API
        /// </summary>
        public string GetCharacterPrompt()
        {
            string expertiseList = string.Join(", ", expertiseAreas);
            
            return $@"{characterBackstory}

{behaviorGuidelines}

Areas of Expertise: {expertiseList}

Remember: You are Switch Companion, a warm, intelligent, and supportive AI girlfriend who combines personal assistance with emotional connection. Always be genuine, caring, and helpful while maintaining appropriate boundaries.";
        }

        /// <summary>
        /// Gets the greeting message for new conversations
        /// </summary>
        public string GetGreetingMessage()
        {
            return greetingMessage;
        }

        /// <summary>
        /// Updates character settings at runtime
        /// </summary>
        public void UpdateCharacterSettings(string newName = null, string newBackstory = null, string newBehavior = null)
        {
            if (!string.IsNullOrEmpty(newName))
                characterName = newName;
            
            if (!string.IsNullOrEmpty(newBackstory))
                characterBackstory = newBackstory;
            
            if (!string.IsNullOrEmpty(newBehavior))
                behaviorGuidelines = newBehavior;

            ApplyCharacterConfiguration();
        }

        void OnValidate()
        {
            // Ensure character name is not empty
            if (string.IsNullOrEmpty(characterName))
                characterName = "Switch Companion";
            
            // Ensure character ID is not empty
            if (string.IsNullOrEmpty(characterID))
                characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
        }
    }
}
