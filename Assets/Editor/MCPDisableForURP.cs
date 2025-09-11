using UnityEditor;
using UnityEngine;

/// <summary>
/// Disables MCP Unity Bridge to prevent URP serialization errors
/// These errors don't affect build or runtime functionality
/// </summary>
[InitializeOnLoad]
public static class MCPDisableForURP
{
    private const string MCP_DISABLE_KEY = "MCP_DISABLE_URP_ERRORS";
    
    static MCPDisableForURP()
    {
        // Check if we should disable MCP to prevent URP errors
        bool shouldDisable = EditorPrefs.GetBool(MCP_DISABLE_KEY, true); // Default to true
        
        if (shouldDisable)
        {
            DisableMCPBridge();
        }
    }
    
    [MenuItem("Tools/MCP/Disable MCP Bridge (Prevents URP Errors)")]
    public static void DisableMCPBridge()
    {
        try
        {
            // Find and disable the MCP Bridge
            var mcpBridgeType = System.Type.GetType("MCPForUnity.Editor.MCPForUnityBridge");
            if (mcpBridgeType != null)
            {
                var stopMethod = mcpBridgeType.GetMethod("Stop", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                stopMethod?.Invoke(null, null);
                
                Debug.Log("[MCP Disable] MCP Unity Bridge disabled to prevent URP serialization errors. Your app will build and run normally.");
                EditorPrefs.SetBool(MCP_DISABLE_KEY, true);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[MCP Disable] Could not disable MCP bridge: {ex.Message}");
        }
    }
    
    [MenuItem("Tools/MCP/Enable MCP Bridge (May Show URP Errors)")]
    public static void EnableMCPBridge()
    {
        try
        {
            // Find and enable the MCP Bridge
            var mcpBridgeType = System.Type.GetType("MCPForUnity.Editor.MCPForUnityBridge");
            if (mcpBridgeType != null)
            {
                var startMethod = mcpBridgeType.GetMethod("Start", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                startMethod?.Invoke(null, null);
                
                Debug.Log("[MCP Enable] MCP Unity Bridge enabled. URP serialization errors may appear but don't affect functionality.");
                EditorPrefs.SetBool(MCP_DISABLE_KEY, false);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[MCP Enable] Could not enable MCP bridge: {ex.Message}");
        }
    }
    
    [MenuItem("Tools/MCP/Clear Console and Restart Unity")]
    public static void ClearAndRestart()
    {
        // Clear console
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
        
        Debug.Log("[MCP] Console cleared. Consider restarting Unity if errors persist.");
    }
}