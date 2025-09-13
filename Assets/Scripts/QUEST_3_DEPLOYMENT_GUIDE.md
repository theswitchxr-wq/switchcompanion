# Meta Quest 3 Deployment Guide for Shafina Spatial Awareness

## Overview
This guide covers building and deploying the Shafina spatial awareness system to Meta Quest 3 for testing and validation.

## Prerequisites
- Unity 2022.3 LTS or later
- Meta Quest 3 device
- Meta Quest Developer Hub installed
- Android SDK and NDK configured
- USB debugging enabled on Quest 3

## Pre-Build Setup

### 1. Unity Build Settings
1. **Platform**: Switch to Android
2. **Architecture**: ARM64
3. **Scripting Backend**: IL2CPP
4. **Target API Level**: 33 (Android 13) or higher
5. **Minimum API Level**: 23 (Android 6.0)

### 2. XR Settings Configuration
1. **XR Plug-in Management**:
   - Enable "Meta OpenXR"
   - Disable other XR providers
2. **OpenXR Features**:
   - Enable "Meta Quest: AR Session"
   - Enable "Meta Quest: AR Anchors"
   - Enable "Meta Quest: AR Plane Detection"
   - Enable "Meta Quest: AR Raycasts"
   - Enable "Meta Quest: AR Camera (Passthrough)"

### 3. Player Settings
1. **Company Name**: Your company name
2. **Product Name**: "Shafina Spatial MR"
3. **Package Name**: com.yourcompany.shafina.spatial
4. **Version**: 1.0.0
5. **Minimum API Level**: 23
6. **Target API Level**: 33

### 4. Quality Settings
1. **Quality Level**: Medium (for Quest 3 performance)
2. **Anti-aliasing**: 2x Multi Sampling
3. **Anisotropic Textures**: Per Texture
4. **Texture Quality**: Half Res

## Build Process

### 1. Scene Setup
1. Open `Assets/Scenes/SampleScene.unity`
2. Add `ShafinaSpatialIntegration` component to any GameObject
3. Verify all spatial awareness components are present:
   - ShafinaSpatialAwareness
   - ShafinaSpatialDialogue
   - ShafinaSpatialManager
   - ARFoundationSetup

### 2. Build Configuration
1. **File → Build Settings**
2. **Platform**: Android
3. **Scenes**: Add SampleScene.unity
4. **Build System**: Gradle
5. **Development Build**: Check (for debugging)
6. **Script Debugging**: Check (for debugging)

### 3. Build Steps
1. Click "Build"
2. Choose build location (e.g., `Builds/Quest3/`)
3. Wait for build to complete
4. Build APK when prompted

## Deployment

### 1. Connect Quest 3
1. Enable Developer Mode on Quest 3
2. Connect via USB-C cable
3. Allow USB debugging when prompted
4. Verify connection in Meta Quest Developer Hub

### 2. Install APK
```bash
# Using ADB
adb install -r "path/to/your/app.apk"

# Or use Meta Quest Developer Hub
# Drag and drop APK file
```

### 3. Launch Application
1. Put on Quest 3 headset
2. Find "Shafina Spatial MR" in apps
3. Launch the application
4. Grant necessary permissions (camera, storage)

## Testing Protocol

### 1. Basic Functionality Test
1. **Launch Test**: Verify app starts without crashes
2. **Avatar Test**: Confirm Shafina appears and is interactive
3. **Convai Test**: Test voice interaction with Shafina
4. **UI Test**: Verify all UI elements are visible and functional

### 2. Spatial Awareness Test
1. **Room Scanning**: Move around the room to scan environment
2. **Wall Detection**: Walk near walls and verify detection
3. **Floor Detection**: Look down and verify floor detection
4. **Furniture Detection**: Walk near furniture and verify detection
5. **Reaction Test**: Listen for Shafina's spatial reactions

### 3. Performance Test
1. **Frame Rate**: Monitor for consistent 72fps
2. **Memory Usage**: Check for memory leaks
3. **Battery Life**: Monitor battery consumption
4. **Thermal**: Check for overheating

### 4. Edge Case Testing
1. **Low Light**: Test in dimly lit environments
2. **Cluttered Space**: Test in rooms with many objects
3. **Empty Space**: Test in large, empty rooms
4. **Movement**: Test while walking around

## Debugging

### 1. Console Logs
```bash
# View Unity logs
adb logcat -s Unity

# Filter for spatial awareness logs
adb logcat | grep "ShafinaSpatial"
```

### 2. Common Issues and Solutions

#### Issue: No spatial detection
- **Cause**: AR Foundation not properly configured
- **Solution**: Verify OpenXR settings and permissions

#### Issue: Shafina doesn't react
- **Cause**: Convai integration not working
- **Solution**: Check Convai API key and network connection

#### Issue: Poor performance
- **Cause**: Too many visual indicators or high detection frequency
- **Solution**: Reduce detection range or increase detection interval

#### Issue: App crashes on launch
- **Cause**: Missing dependencies or incorrect build settings
- **Solution**: Check build configuration and dependencies

### 3. Performance Optimization
1. **Reduce Detection Range**: Lower from 3m to 2m
2. **Increase Detection Interval**: Change from 0.5s to 1.0s
3. **Limit Visual Indicators**: Reduce maximum objects
4. **Disable Debug Features**: Turn off debug rays and logs

## Validation Checklist

### ✅ Core Features
- [ ] Shafina avatar appears and is interactive
- [ ] Convai voice interaction works
- [ ] Spatial detection functions properly
- [ ] Visual indicators appear for detected objects
- [ ] Shafina reacts to spatial objects with dialogue
- [ ] AR plane detection works
- [ ] Raycast detection works

### ✅ Performance
- [ ] Consistent 72fps on Quest 3
- [ ] No significant frame drops
- [ ] Memory usage stays stable
- [ ] No overheating issues
- [ ] Battery life is acceptable

### ✅ User Experience
- [ ] Intuitive controls
- [ ] Clear visual feedback
- [ ] Responsive interactions
- [ ] Smooth movement
- [ ] Clear audio

### ✅ Stability
- [ ] No crashes during normal use
- [ ] Handles edge cases gracefully
- [ ] Recovers from errors
- [ ] Maintains state properly

## Post-Deployment

### 1. Data Collection
- Monitor user interactions
- Collect performance metrics
- Gather feedback on spatial reactions
- Track detection accuracy

### 2. Iteration
- Refine detection algorithms
- Improve reaction timing
- Optimize performance
- Add new features based on feedback

### 3. Scaling
- Test on different room types
- Validate with multiple users
- Optimize for different Quest models
- Prepare for production release

## Troubleshooting

### Build Issues
- Verify Android SDK installation
- Check Unity version compatibility
- Ensure all dependencies are included
- Validate build settings

### Runtime Issues
- Check device compatibility
- Verify permissions
- Monitor system resources
- Review error logs

### Performance Issues
- Profile the application
- Optimize rendering
- Reduce computational load
- Adjust quality settings

## Support

For technical support:
1. Check Unity Console for errors
2. Review Meta Quest Developer documentation
3. Test in Meta Quest Developer Hub
4. Contact development team with specific error details

## Future Enhancements

- Integration with Meta's Scene API
- Advanced object recognition
- Multi-user spatial sharing
- Gesture-based interactions
- Dynamic room mapping
- AI-powered spatial understanding
