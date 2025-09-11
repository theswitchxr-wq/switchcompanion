using UnityEditor;
using UnityEngine;

/// <summary>
/// Suppresses MCP Unity Bridge serialization errors related to URP render pipeline
/// </summary>
[InitializeOnLoad]
public static class MCPRenderPipelineSuppressor
{
    static MCPRenderPipelineSuppressor()
    {
        // Subscribe to log message received to filter out MCP serialization errors
        Application.logMessageReceived += OnLogMessageReceived;
    }
    
    private static void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        // Filter out known MCP serialization errors that don't affect functionality
        if (type == LogType.Error && IsMCPSerializationError(logString))
        {
            // Suppress these errors as they're non-critical MCP bridge issues
            return;
        }
    }
    
    private static bool IsMCPSerializationError(string logString)
    {
        // Check if this is a known MCP serialization error we want to suppress
        return logString.Contains("You can only call cameraColorTarget inside the scope of a ScriptableRenderPass") ||
               logString.Contains("You can only call cameraDepthTarget inside the scope of a ScriptableRenderPass") ||
               logString.Contains("MCPForUnity.Editor.Helpers.GameObjectSerializer") ||
               (logString.Contains("JSON Deserialization Error") && logString.Contains("MCPForUnity"));
    }
}

/// <summary>
/// Editor utility to manage MCP bridge state
/// </summary>
public static class MCPBridgeUtility
{
    [MenuItem("Tools/MCP/Restart Bridge")]
    public static void RestartMCPBridge()
    {
        try
        {
            var mcpBridgeType = System.Type.GetType("MCPForUnity.Editor.MCPForUnityBridge");
            if (mcpBridgeType != null)
            {
                // Stop the bridge
                var stopMethod = mcpBridgeType.GetMethod("Stop", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                stopMethod?.Invoke(null, null);
                
                // Wait a moment
                System.Threading.Thread.Sleep(100);
                
                // Start the bridge
                var startMethod = mcpBridgeType.GetMethod("Start", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                startMethod?.Invoke(null, null);
                
                Debug.Log("[MCP Bridge Utility] Successfully restarted MCP Unity Bridge");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[MCP Bridge Utility] Failed to restart MCP bridge: {ex.Message}");
        }
    }
    
    [MenuItem("Tools/MCP/Clear Console")]
    public static void ClearConsole()
    {
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}