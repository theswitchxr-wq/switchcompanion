# Quest Testing Guide for Convai MR Microphone

## Overview
This guide explains how to test the microphone functionality on Meta Quest using the grip button for push-to-talk.

## Setup Instructions

### 1. Scene Setup
- Ensure you have a `ConvaiMRMicrophoneInput` component in your scene
- Make sure the `ConvaiInputManager` GameObject is present
- Verify that a `ConvaiNPC` is in the scene and assigned to the microphone input script

### 2. Input Configuration
The microphone input now supports both PC and Quest:
- **PC**: T key for push-to-talk
- **Quest**: Grip button (left or right controller) for push-to-talk

### 3. Testing on PC (Editor)
1. Press **Play** in Unity
2. Press and hold **T** key to start talking
3. Release **T** key to stop talking
4. You should see "LISTENING" status in the UI

### 4. Testing on Quest
1. Build and deploy to Quest
2. Put on the Quest headset
3. Press and hold the **grip button** on either controller
4. Speak into the microphone
5. Release the grip button to stop

## Debug Information

### QuestInputTester Component
- Add the `QuestInputTester` component to any GameObject for debugging
- Press **F1** in the editor to toggle debug info
- Shows grip button status and input action state

### Console Logs
Look for these debug messages:
- `[ConvaiMR] Microphone input initialized successfully!`
- `[ConvaiMR] Quest Support: Enabled (Grip button)`
- `[ConvaiMR] Started listening to NPC`
- `[ConvaiMR] Stopped listening to NPC`

## Troubleshooting

### Green Dot Appears but No Response
- Check that the `ConvaiNPC` is properly assigned
- Verify the `ConvaiInputManager` is in the scene
- Ensure the microphone permissions are granted on Quest

### Grip Button Not Working
- Verify the input actions are loaded correctly
- Check that XR is properly configured in Project Settings
- Ensure the Quest controllers are connected and tracked

### Microphone Not Detected
- Check Quest microphone permissions
- Verify the microphone device is selected in Convai settings
- Test with a simple microphone test first

## Input Action Configuration

The `Controls.inputactions` file now includes:
- **Keyboard**: T key for PC testing
- **XR Controllers**: Left and right grip buttons for Quest

## Visual Feedback

The UI will show:
- Current input method (PC/Quest)
- Listening status
- Target NPC name

## Performance Notes

- The system uses Unity's new Input System for better performance
- Input is processed once per frame for optimal responsiveness
- Both PC and Quest input work simultaneously for testing

## Next Steps

1. Test on Quest device
2. Verify microphone quality and responsiveness
3. Adjust voice activation threshold if needed
4. Test with different Quest controllers (if available)
