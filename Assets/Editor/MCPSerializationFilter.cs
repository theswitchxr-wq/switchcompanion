using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[InitializeOnLoad]
public class MCPSerializationFilter
{
    // Properties that should be excluded from MCP serialization to prevent errors
    private static readonly HashSet<string> ExcludedProperties = new HashSet<string>
    {
        "cameraColorTarget",
        "cameraColorTargetHandle", 
        "cameraDepthTarget",
        "cameraDepthTargetHandle",
        "scriptableRenderer",
        "volumeStack",
        "playableGraph"
    };
    
    // Component types that should be excluded from detailed serialization
    private static readonly HashSet<string> ExcludedComponentTypes = new HashSet<string>
    {
        "UnityEngine.Rendering.Universal.UniversalAdditionalCameraData",
        "UnityEngine.Rendering.Universal.ScriptableRenderer"
    };
    
    static MCPSerializationFilter()
    {
        // This runs when Unity starts up in the editor
        Debug.Log("[MCP Serialization Filter] Initialized - filtering problematic render pipeline properties");
    }
    
    public static bool ShouldExcludeProperty(string componentType, string propertyName)
    {
        // Exclude entire component types that are problematic
        if (ExcludedComponentTypes.Contains(componentType))
        {
            return true;
        }
        
        // Exclude specific properties that cause serialization issues
        if (ExcludedProperties.Contains(propertyName))
        {
            return true;
        }
        
        return false;
    }
    
    public static bool ShouldExcludeComponent(string componentType)
    {
        return ExcludedComponentTypes.Contains(componentType);
    }
}

// Custom property drawer to suppress render pipeline warnings
[CustomPropertyDrawer(typeof(UnityEngine.Rendering.Universal.UniversalAdditionalCameraData))]
public class SafeUniversalCameraDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Safely draw without accessing problematic properties
        EditorGUI.LabelField(position, label.text, "Universal Render Pipeline Camera Data");
    }
}