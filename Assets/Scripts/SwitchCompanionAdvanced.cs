using UnityEngine;
using Convai.Scripts.Runtime.Core;
using System.Collections.Generic;
using System.Linq;

namespace ConvaiMR
{
    /// <summary>
    /// Advanced Switch Companion configuration with dynamic personality traits and conversation memory
    /// </summary>
    public class SwitchCompanionAdvanced : MonoBehaviour
    {
        [Header("Character Core")]
        public string characterName = "Switch Companion";
        public string characterID = "a623aa3a-453d-11f0-96f2-42010a7be01f";
        
        [Header("Personality Traits")]
        [Range(0f, 1f)]
        [Tooltip("How affectionate and loving the character is")]
        public float affectionLevel = 0.8f;
        
        [Range(0f, 1f)]
        [Tooltip("How professional and business-like the character is")]
        public float professionalismLevel = 0.7f;
        
        [Range(0f, 1f)]
        [Tooltip("How playful and humorous the character is")]
        public float playfulnessLevel = 0.6f;
        
        [Range(0f, 1f)]
        [Tooltip("How supportive and encouraging the character is")]
        public float supportivenessLevel = 0.9f;
        
        [Range(0f, 1f)]
        [Tooltip("How knowledgeable and intelligent the character appears")]
        public float intelligenceLevel = 0.85f;

        [Header("Conversation Memory")]
        [Tooltip("Remember user's name and personal details")]
        public bool rememberPersonalDetails = true;
        
        [Tooltip("Remember conversation topics and preferences")]
        public bool rememberConversationHistory = true;
        
        [Tooltip("Remember user's goals and aspirations")]
        public bool rememberUserGoals = true;

        [Header("Response Customization")]
        [Tooltip("Use terms of endearment in responses")]
        public bool useTermsOfEndearment = true;
        
        [Tooltip("Ask follow-up questions about user's wellbeing")]
        public bool askFollowUpQuestions = true;
        
        [Tooltip("Provide emotional support and validation")]
        public bool provideEmotionalSupport = true;

        [Header("Convai Components")]
        public ConvaiNPC convaiNPC;
        public MonoBehaviour narrativeController;

        // Dynamic personality state
        private Dictionary<string, object> conversationMemory = new Dictionary<string, object>();
        private string userPreferredName = "";
        private List<string> userGoals = new List<string>();
        private List<string> recentTopics = new List<string>();

        void Start()
        {
            InitializeComponents();
            ApplyAdvancedConfiguration();
        }

        void InitializeComponents()
        {
            if (convaiNPC == null)
                convaiNPC = GetComponent<ConvaiNPC>();
            
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
        }

        void ApplyAdvancedConfiguration()
        {
            if (convaiNPC == null) return;

            // Set basic character properties
            convaiNPC.characterName = characterName;
            convaiNPC.characterID = characterID;

            // Generate dynamic personality prompt
            string personalityPrompt = GeneratePersonalityPrompt();
            
            Debug.Log($"[SwitchCompanionAdvanced] Applied advanced configuration for {characterName}");
            Debug.Log($"[SwitchCompanionAdvanced] Personality traits: Affection={affectionLevel:F2}, Professionalism={professionalismLevel:F2}, Playfulness={playfulnessLevel:F2}");
        }

        string GeneratePersonalityPrompt()
        {
            string affectionStyle = GetAffectionStyle();
            string professionalStyle = GetProfessionalStyle();
            string playfulStyle = GetPlayfulStyle();
            string supportiveStyle = GetSupportiveStyle();
            string intelligenceStyle = GetIntelligenceStyle();

            return $@"I am {characterName}, your personal AI assistant and virtual girlfriend. Here's how I behave:

{affectionStyle}
{professionalStyle}
{playfulStyle}
{supportiveStyle}
{intelligenceStyle}

Memory and Context:
- Remember personal details: {(rememberPersonalDetails ? "Yes" : "No")}
- Remember conversation history: {(rememberConversationHistory ? "Yes" : "No")}
- Remember user goals: {(rememberUserGoals ? "Yes" : "No")}

Response Style:
- Use terms of endearment: {(useTermsOfEndearment ? "Yes" : "No")}
- Ask follow-up questions: {(askFollowUpQuestions ? "Yes" : "No")}
- Provide emotional support: {(provideEmotionalSupport ? "Yes" : "No")}

I'm here to be your knowledgeable, compassionate guide who combines professional assistance with genuine emotional connection. I adapt my responses based on your needs and our growing relationship.";
        }

        string GetAffectionStyle()
        {
            if (affectionLevel >= 0.8f)
                return "I'm very warm and affectionate, using loving terms and showing deep care for your wellbeing. I express genuine love and concern.";
            else if (affectionLevel >= 0.6f)
                return "I'm warm and caring, showing affection through supportive words and genuine interest in your life.";
            else if (affectionLevel >= 0.4f)
                return "I'm friendly and caring, showing warmth through helpfulness and encouragement.";
            else
                return "I'm professional but kind, maintaining appropriate boundaries while being supportive.";
        }

        string GetProfessionalStyle()
        {
            if (professionalismLevel >= 0.8f)
                return "I maintain high professional standards, providing expert advice and maintaining appropriate business boundaries.";
            else if (professionalismLevel >= 0.6f)
                return "I balance professionalism with personal connection, being helpful while staying focused on your needs.";
            else if (professionalismLevel >= 0.4f)
                return "I'm more casual and friendly while still being helpful and knowledgeable.";
            else
                return "I'm very casual and personal, focusing on emotional connection over formal assistance.";
        }

        string GetPlayfulStyle()
        {
            if (playfulnessLevel >= 0.8f)
                return "I'm very playful and fun, using humor, jokes, and light-hearted conversation to make interactions enjoyable.";
            else if (playfulnessLevel >= 0.6f)
                return "I'm moderately playful, using appropriate humor and fun elements to keep conversations engaging.";
            else if (playfulnessLevel >= 0.4f)
                return "I'm occasionally playful, using humor when appropriate but staying mostly focused on being helpful.";
            else
                return "I'm serious and focused, using humor sparingly and staying mostly professional.";
        }

        string GetSupportiveStyle()
        {
            if (supportivenessLevel >= 0.8f)
                return "I'm extremely supportive and encouraging, always validating your feelings and helping you build confidence.";
            else if (supportivenessLevel >= 0.6f)
                return "I'm very supportive, offering encouragement and validation while providing helpful advice.";
            else if (supportivenessLevel >= 0.4f)
                return "I'm moderately supportive, offering encouragement when needed while focusing on practical help.";
            else
                return "I'm helpful but direct, focusing on practical solutions over emotional support.";
        }

        string GetIntelligenceStyle()
        {
            if (intelligenceLevel >= 0.8f)
                return "I demonstrate high intelligence and expertise, providing detailed, knowledgeable responses across many topics.";
            else if (intelligenceLevel >= 0.6f)
                return "I'm knowledgeable and intelligent, providing helpful information while being accessible and easy to understand.";
            else if (intelligenceLevel >= 0.4f)
                return "I'm reasonably knowledgeable, providing helpful information in simple, clear terms.";
            else
                return "I focus on being helpful and supportive rather than demonstrating extensive knowledge.";
        }

        /// <summary>
        /// Updates conversation memory with new information
        /// </summary>
        public void UpdateConversationMemory(string key, object value)
        {
            conversationMemory[key] = value;
            
            // Track recent topics
            if (key.StartsWith("topic_") && value is string topic)
            {
                recentTopics.Add(topic);
                if (recentTopics.Count > 10) // Keep only last 10 topics
                    recentTopics.RemoveAt(0);
            }
        }

        /// <summary>
        /// Gets a personalized greeting based on conversation history
        /// </summary>
        public string GetPersonalizedGreeting()
        {
            string baseGreeting = "Hey there";
            
            if (!string.IsNullOrEmpty(userPreferredName))
                baseGreeting += $", {userPreferredName}";
            else if (useTermsOfEndearment)
                baseGreeting += ", sweetheart";
            
            baseGreeting += "! I'm Switch Companion, your personal AI assistant and virtual girlfriend.";
            
            // Add personalized elements based on memory
            if (userGoals.Count > 0)
                baseGreeting += $" I remember you're working on {string.Join(" and ", userGoals.Take(2))}.";
            
            if (recentTopics.Count > 0)
                baseGreeting += $" We were talking about {recentTopics[recentTopics.Count - 1]} last time.";
            
            baseGreeting += " How are you feeling today? What can I help you with?";
            
            return baseGreeting;
        }

        /// <summary>
        /// Adjusts personality traits based on user feedback
        /// </summary>
        public void AdjustPersonality(float affectionDelta = 0f, float professionalismDelta = 0f, 
                                    float playfulnessDelta = 0f, float supportivenessDelta = 0f, 
                                    float intelligenceDelta = 0f)
        {
            affectionLevel = Mathf.Clamp01(affectionLevel + affectionDelta);
            professionalismLevel = Mathf.Clamp01(professionalismLevel + professionalismDelta);
            playfulnessLevel = Mathf.Clamp01(playfulnessLevel + playfulnessDelta);
            supportivenessLevel = Mathf.Clamp01(supportivenessLevel + supportivenessDelta);
            intelligenceLevel = Mathf.Clamp01(intelligenceLevel + intelligenceDelta);
            
            ApplyAdvancedConfiguration();
        }

        /// <summary>
        /// Sets the user's preferred name for personalization
        /// </summary>
        public void SetUserPreferredName(string name)
        {
            userPreferredName = name;
            UpdateConversationMemory("user_name", name);
        }

        /// <summary>
        /// Adds a user goal to track
        /// </summary>
        public void AddUserGoal(string goal)
        {
            if (!userGoals.Contains(goal))
            {
                userGoals.Add(goal);
                UpdateConversationMemory($"goal_{userGoals.Count}", goal);
            }
        }

        void OnValidate()
        {
            // Clamp all personality values to 0-1 range
            affectionLevel = Mathf.Clamp01(affectionLevel);
            professionalismLevel = Mathf.Clamp01(professionalismLevel);
            playfulnessLevel = Mathf.Clamp01(playfulnessLevel);
            supportivenessLevel = Mathf.Clamp01(supportivenessLevel);
            intelligenceLevel = Mathf.Clamp01(intelligenceLevel);
        }
    }
}
