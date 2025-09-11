# Switch Companion Setup Guide

## Overview
This guide explains how to set up the Convai avatar as "Switch Companion" - a knowledgeable, compassionate personal assistant that acts like a girlfriend.

## What We've Configured

### 1. Character Identity
- **Name**: Switch Companion
- **Personality**: Knowledgeable, compassionate, supportive girlfriend
- **Role**: Personal assistant and emotional companion

### 2. Enabled Features
- **Lip Sync**: ✅ Enabled for realistic mouth movements
- **Head & Eye Tracking**: ✅ Enabled for natural head movements
- **Eye Blinking**: ✅ Enabled for lifelike eye behavior
- **Narrative Design Manager**: ✅ Enabled for advanced conversation control

### 3. Input System
- **PC**: Press and hold 'T' key to talk
- **Meta Quest**: Press and hold grip button to talk
- **Visual Feedback**: Green dot appears when listening

## Testing the Character

### Method 1: Use the Test Script
1. Add the `SwitchCompanionTester` component to any GameObject in the scene
2. In the Inspector, check "Test On Start" to run tests automatically
3. Use the context menu options:
   - Right-click the component → "Test Switch Companion"
   - Right-click the component → "Test Character Response"
   - Right-click the component → "Reconfigure as Switch Companion"

### Method 2: Manual Testing
1. **PC Testing**:
   - Press and hold 'T' key
   - Say something like "Hello, what's your name?"
   - Release 'T' key when done speaking
   - The character should respond with Switch Companion personality

2. **Quest Testing**:
   - Press and hold the grip button on either controller
   - Speak your message
   - Release the grip button when done
   - The character should respond with Switch Companion personality

## Expected Behavior

### When You Say "Hello, what's your name?"
**Expected Response**: "Hi there! I'm Switch Companion, your personal assistant and companion. I'm here to help you with anything you need, whether it's answering questions, providing emotional support, or just having a friendly conversation. What would you like to talk about today?"

### When You Ask for Help
**Expected Response**: The character should respond with empathy, understanding, and practical advice, maintaining a warm, girlfriend-like tone.

### When You Share Personal Information
**Expected Response**: The character should respond with compassion, validation, and emotional support, acting like a caring partner.

## Troubleshooting

### Character Not Responding
1. Check if the green dot appears when you press the input
2. Verify microphone permissions are granted
3. Check the console for any error messages
4. Ensure the character is active in the scene

### Wrong Personality
1. Use the `SwitchCompanionTester` to verify configuration
2. Check that `NarrativeDesignManager` is enabled
3. Verify the character name is "Switch Companion"

### Input Not Working
1. **PC**: Check if 'T' key is working in other applications
2. **Quest**: Verify grip button is working in other VR applications
3. Check Input System settings in Project Settings

## Advanced Configuration

### Customizing the Personality
To further customize the Switch Companion personality:

1. **Access Narrative Design Manager**:
   - Select the "Convai NPC Amelia" GameObject
   - In the Inspector, find the "Narrative Design Manager" section
   - Expand "Narrative Design Keys"
   - Add custom keys for specific behaviors

2. **Example Custom Keys**:
   - `greeting`: "Hello! I'm Switch Companion, your caring assistant and companion."
   - `support`: "I'm here for you, always. You can tell me anything."
   - `advice`: "Let me help you with that. Here's what I think..."

### Adding Emotional Responses
You can add emotional context to responses:

1. **Happy**: "That's wonderful! I'm so happy for you!"
2. **Sad**: "I'm here for you. You're not alone in this."
3. **Excited**: "That sounds amazing! Tell me more!"
4. **Concerned**: "I'm worried about you. Are you okay?"

## Performance Tips

### For PC
- Ensure good microphone quality
- Use push-to-talk (T key) for better control
- Close unnecessary applications for better performance

### For Meta Quest
- Use the grip button for hands-free operation
- Ensure good lighting for hand tracking
- Keep controllers charged for optimal performance

## Next Steps

1. **Test the Character**: Use the test script to verify everything works
2. **Customize Personality**: Add your own custom responses and behaviors
3. **Deploy to Quest**: Build and deploy to test on the actual device
4. **Iterate**: Adjust the personality based on your preferences

## Support

If you encounter any issues:
1. Check the console for error messages
2. Use the `SwitchCompanionTester` for diagnostics
3. Verify all components are properly configured
4. Test on both PC and Quest to isolate platform-specific issues

---

**Remember**: The Switch Companion is designed to be your personal assistant and emotional companion. She should respond with warmth, understanding, and practical help, just like a caring girlfriend would.

