# Shafina Spatial Awareness Setup Guide

## Overview
This guide explains how to set up complete spatial awareness for Shafina (Convai Ninja avatar) in Mixed Reality on Meta Quest 3. The system enables Shafina to detect walls, floors, furniture, and other real-world objects, allowing for contextual interactions and reactions.

## Features
- **Real-time spatial detection** using AR Foundation and Meta Quest 3's spatial features
- **Contextual reactions** - Shafina responds to detected objects with appropriate dialogue
- **Visual indicators** - Shows detected surfaces with colored materials
- **AR plane integration** - Works with Meta's scene understanding
- **Raycast-based detection** - Additional physics-based object detection
- **Configurable reactions** - Customize Shafina's responses to different object types

## Quick Setup

### 1. Automatic Setup (Recommended)
1. Add the `ShafinaSpatialSetup` component to any GameObject in your scene
2. Click "Setup Spatial Awareness" in the inspector or run the scene
3. The system will automatically:
   - Find your Convai avatar
   - Set up AR Foundation components
   - Configure spatial awareness
   - Load materials and prefabs

### 2. Manual Setup
If you prefer manual setup or need to customize specific components:

#### Step 1: AR Foundation Setup
1. Add `ARFoundationSetup` component to a GameObject
2. Ensure AR Session, AR Session Origin, AR Plane Manager, and AR Raycast Manager are configured
3. Set up plane detection for horizontal and vertical surfaces

#### Step 2: Avatar Configuration
1. Find your Convai NPC avatar in the scene
2. Add `ShafinaSpatialAwareness` component
3. Configure detection settings:
   - Detection Range: 3.0 (meters)
   - Detection Interval: 0.5 (seconds)
   - Spatial Layer Mask: All layers (-1)

#### Step 3: Dialogue Integration
1. Add `ShafinaSpatialDialogue` component to the same avatar
2. Configure reaction settings:
   - Reaction Cooldown: 5.0 (seconds)
   - Enable Voice Reactions: True
   - Enable Text Reactions: True

#### Step 4: Materials and Prefabs
1. Assign materials to the spatial awareness component:
   - Wall Material: Blue semi-transparent
   - Floor Material: Green semi-transparent  
   - Furniture Material: Orange semi-transparent
2. Assign the SpatialIndicator prefab for visual feedback

## Configuration Options

### ShafinaSpatialAwareness Settings
- **Detection Range**: How far Shafina can detect objects (default: 3m)
- **Detection Interval**: How often to check for new objects (default: 0.5s)
- **Spatial Layer Mask**: Which layers to detect objects on (default: All)
- **Show Debug Rays**: Visualize detection rays in scene view
- **Enable Spatial Reactions**: Allow Shafina to react to detected objects

### ShafinaSpatialDialogue Settings
- **Reaction Cooldown**: Minimum time between reactions to same object type
- **Enable Voice Reactions**: Play audio responses
- **Enable Text Reactions**: Display text responses
- **Spatial Reactions**: Customize reactions for each object type

### AR Foundation Settings
- **Plane Detection Mode**: Horizontal and Vertical
- **Plane Visualization**: Enable/disable visual plane rendering
- **Raycast Manager**: Enable for additional object detection

## Object Types Detected

### Wall
- **Detection**: Surfaces with mostly horizontal normals
- **Reaction**: "I can see a wall here. This helps me understand the room layout!"
- **Visual**: Blue semi-transparent indicator

### Floor
- **Detection**: Surfaces with upward-pointing normals
- **Reaction**: "This is the floor. I can walk around here safely."
- **Visual**: Green semi-transparent indicator

### Ceiling
- **Detection**: Surfaces with downward-pointing normals
- **Reaction**: "I can see the ceiling above me."
- **Visual**: Default indicator

### Furniture
- **Detection**: Other detected objects not classified as walls/floors
- **Reaction**: "I notice some furniture nearby. I'll be careful around it."
- **Visual**: Orange semi-transparent indicator

## Testing

### In Editor
1. Use the "Test Spatial Awareness" context menu option
2. Check the console for detection logs
3. Verify visual indicators appear in scene view

### On Meta Quest 3
1. Build and deploy to Quest 3
2. Move around the real environment
3. Watch for Shafina's reactions to walls, floors, and furniture
4. Check console logs for detection information

## Troubleshooting

### No Objects Detected
- Check that AR Foundation is properly configured
- Verify the spatial layer mask includes the correct layers
- Ensure objects have colliders
- Check detection range settings

### No Reactions from Shafina
- Verify ConvaiNPC component is present and configured
- Check that ShafinaSpatialDialogue is enabled
- Ensure reaction cooldown isn't too high
- Verify audio source is configured

### Performance Issues
- Increase detection interval
- Reduce detection range
- Limit spatial layer mask to necessary layers
- Disable debug visualization

### AR Foundation Issues
- Ensure Meta Quest 3 is properly configured in XR settings
- Check that AR Session is running
- Verify plane detection permissions are granted
- Test with a well-lit environment

## Advanced Customization

### Custom Reactions
Modify the `SpatialReaction` array in `ShafinaSpatialDialogue` to add custom responses:

```csharp
new SpatialReaction
{
    objectType = ShafinaSpatialAwareness.SpatialObjectType.Wall,
    reactionTexts = new string[] { "Custom wall reaction!" },
    priority = 1f
}
```

### Custom Detection Logic
Override the `ClassifySpatialObject` method in `ShafinaSpatialAwareness` to add custom object classification.

### Integration with Convai Dialogue
To integrate with Convai's actual dialogue system, modify the `SimulateDialogueReaction` method in `ShafinaSpatialDialogue` to use Convai's API.

## Performance Considerations

- Detection runs every 0.5 seconds by default
- Visual indicators are created for each detected object
- Consider object pooling for better performance
- Use LOD system for distant objects
- Limit maximum number of active indicators

## Future Enhancements

- Pathfinding around detected obstacles
- Gesture-based interactions with real objects
- Dynamic room mapping and navigation
- Integration with Meta's Scene API
- Advanced object recognition using AI
- Multi-user spatial awareness sharing

## Support

For issues or questions:
1. Check the console logs for error messages
2. Verify all components are properly configured
3. Test in a simple environment first
4. Ensure Meta Quest 3 firmware is up to date
