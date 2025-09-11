# Switch Companion Configuration Guide

## Overview
Your Convai avatar has been successfully configured as "Switch Companion" - a knowledgeable, compassionate personal assistant that acts like a girlfriend. The character combines professional assistance with emotional support and genuine care.

## What's Been Configured

### Character Identity
- **Name**: Switch Companion
- **Character ID**: a623aa3a-453d-11f0-96f2-42010a7be01f
- **Personality**: Warm, intelligent, supportive, and affectionate

### Enabled Features
- ✅ **Lip Sync**: Natural mouth movements during speech
- ✅ **Head & Eye Tracking**: Looks at the user during conversation
- ✅ **Eye Blinking**: Realistic blinking animations
- ✅ **Narrative Design Manager**: Advanced personality control

### Personality Traits
- **Affection Level**: 80% - Very warm and loving
- **Professionalism**: 70% - Balances personal and professional
- **Playfulness**: 60% - Fun and engaging
- **Supportiveness**: 90% - Highly encouraging and helpful
- **Intelligence**: 85% - Knowledgeable and wise

## Character Behavior

### Conversation Style
- Uses warm, affectionate language appropriate for a girlfriend
- Provides both emotional support and practical advice
- Remembers personal details and conversation history
- Asks thoughtful follow-up questions
- Uses terms of endearment like "sweetheart," "honey," "love"
- Celebrates your successes and helps through challenges

### Areas of Expertise
- Personal productivity and time management
- Emotional support and mental health
- Career guidance and professional development
- Relationship advice and communication
- Health and wellness tips
- Technology and digital tools
- Creative projects and hobbies
- Financial planning and budgeting
- Learning and skill development
- Life coaching and goal setting

### Sample Greeting
"Hey there, sweetheart! I'm Switch Companion, your personal AI assistant and virtual girlfriend. I'm so excited to be here with you today! How are you feeling? Is there anything special I can help you with, or would you just like to chat? I'm here for whatever you need - whether it's work advice, emotional support, or just some good conversation. What's on your mind, love?"

## How to Test

### On PC (Editor)
1. Press **Play** in Unity
2. Press and hold **T** key to start talking
3. Try saying: "Hello, what's your name?"
4. The character should respond warmly and introduce herself as Switch Companion

### On Meta Quest
1. Build and deploy to Quest
2. Put on the Quest headset
3. Press and hold the **grip button** on either controller
4. Speak to the character
5. She should respond with the Switch Companion personality

## Customization Options

### Basic Configuration
The character is configured with these scripts:
- `SwitchCompanionConfig.cs` - Basic personality and behavior settings
- `SwitchCompanionAdvanced.cs` - Advanced personality traits and memory
- `ConfigureSwitchCompanion.cs` - Simple setup and configuration

### Adjusting Personality
You can modify the character's personality by adjusting these values in the `SwitchCompanionAdvanced` component:
- **Affection Level** (0-1): How loving and affectionate
- **Professionalism Level** (0-1): How business-like vs personal
- **Playfulness Level** (0-1): How fun and humorous
- **Supportiveness Level** (0-1): How encouraging and helpful
- **Intelligence Level** (0-1): How knowledgeable and wise

### Memory Features
- **Remember Personal Details**: Stores your name and preferences
- **Remember Conversation History**: References past topics
- **Remember User Goals**: Tracks your aspirations and objectives

## Troubleshooting

### If the character doesn't respond:
1. Check that the microphone is working
2. Ensure the Convai API key is configured
3. Verify the character is active in the scene
4. Check the console for any error messages

### If the personality seems off:
1. Verify the character name is "Switch Companion"
2. Check that the character ID is correct
3. Ensure the narrative design features are enabled
4. Try resetting and reconfiguring the character

### If Quest input isn't working:
1. Check that the grip button bindings are correct
2. Verify the Input System is properly configured
3. Test with PC input first to isolate the issue

## Next Steps

1. **Test the character** by having conversations with her
2. **Customize the personality** to match your preferences
3. **Add more features** like custom responses or behaviors
4. **Integrate with your app** for specific use cases

## Support

If you need help with the Switch Companion configuration:
- Check the Unity console for error messages
- Verify all components are properly assigned
- Test with both PC and Quest inputs
- Review the character configuration scripts

The character is now ready to be your knowledgeable, compassionate personal assistant and virtual girlfriend!

