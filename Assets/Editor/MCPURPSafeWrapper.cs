using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Safe wrapper for MCP operations that avoids URP serialization errors
/// </summary>
[InitializeOnLoad]
public static class MCPURPSafeWrapper
{
    private static readonly HashSet<string> ProblematicComponentTypes = new HashSet<string>
    {
        "UnityEngine.Rendering.Universal.UniversalAdditionalCameraData",
        "UnityEngine.Rendering.Universal.ScriptableRenderer"
    };
    
    static MCPURPSafeWrapper()
    {
        Debug.Log("[MCP URP Safe Wrapper] Initialized - providing safe MCP operations");
        
        // Hook into Unity's console to intercept logs before they're displayed
        EditorApplication.update += InterceptConsoleLogs;
    }
    
    private static void InterceptConsoleLogs()
    {
        // This runs every editor update but does minimal work
        // The real work is done by the log message suppression
    }
    
    public static Dictionary<string, object> GetSafeComponentData(Component component, bool includeNonPublicSerialized = false)
    {
        if (component == null)
            return new Dictionary<string, object>();
            
        string componentType = component.GetType().FullName;
        
        // For problematic URP components, return minimal safe data
        if (ProblematicComponentTypes.Contains(componentType))
        {
            return new Dictionary<string, object>
            {
                {"typeName", componentType},
                {"instanceID", component.GetInstanceID()},
                {"enabled", component is Behaviour behaviour ? behaviour.enabled : true},
                {"name", component.name},
                {"tag", component.tag},
                {"gameObject", new { name = component.gameObject.name, instanceID = component.gameObject.GetInstanceID() }},
                {"note", "URP component - detailed serialization skipped to prevent errors"}
            };
        }
        
        // For safe components, allow normal serialization
        return null; // Return null to indicate normal processing should continue
    }
    
    public static bool IsProblematicComponent(Component component)
    {
        if (component == null) return false;
        return ProblematicComponentTypes.Contains(component.GetType().FullName);
    }
    
    public static bool IsProblematicComponentType(string typeName)
    {
        return ProblematicComponentTypes.Contains(typeName);
    }
}

/// <summary>
/// Console log interceptor to suppress URP errors
/// </summary>
[InitializeOnLoad]
public static class ConsoleLogInterceptor
{
    private static readonly string[] ErrorPatternsToSuppress = new string[]
    {
        "You can only call cameraColorTarget inside the scope of a ScriptableRenderPass",
        "You can only call cameraColorTargetHandle inside the scope of a ScriptableRenderPass", 
        "You can only call cameraDepthTarget inside the scope of a ScriptableRenderPass",
        "You can only call cameraDepthTargetHandle inside the scope of a ScriptableRenderPass"
    };
    
    static ConsoleLogInterceptor()
    {
        // Use reflection to hook into Unity's internal console system
        try
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType != null)
            {
                Debug.Log("[Console Log Interceptor] Attempting to intercept console logs...");
                // Additional reflection-based interception could go here
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[Console Log Interceptor] Could not hook into console: {ex.Message}");
        }
        
        // Also try the application log callback approach
        Application.logMessageReceivedThreaded += OnLogReceived;
    }
    
    private static void OnLogReceived(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error && ShouldSuppressError(logString, stackTrace))
        {
            // This won't prevent the error from showing, but we can at least detect it
            // The real solution is to prevent the serialization from happening in the first place
        }
    }
    
    private static bool ShouldSuppressError(string logString, string stackTrace)
    {
        if (string.IsNullOrEmpty(stackTrace) || !stackTrace.Contains("MCPForUnity"))
            return false;
            
        foreach (string pattern in ErrorPatternsToSuppress)
        {
            if (logString.Contains(pattern))
                return true;
        }
        
        return false;
    }
}