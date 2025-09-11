using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class MCPBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("[MCP Build Processor] Disabling MCP Unity Bridge during build...");
        
        // Disable MCP bridge processing during builds to prevent serialization errors
        EditorPrefs.SetBool("MCPForUnity.DisableDuringBuild", true);
        
        // Try to stop the MCP bridge if it's running
        try
        {
            var mcpBridgeType = System.Type.GetType("MCPForUnity.Editor.MCPForUnityBridge");
            if (mcpBridgeType != null)
            {
                var stopMethod = mcpBridgeType.GetMethod("Stop", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                stopMethod?.Invoke(null, null);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[MCP Build Processor] Could not stop MCP bridge: {ex.Message}");
        }
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("[MCP Build Processor] Re-enabling MCP Unity Bridge after build...");
        
        // Re-enable MCP bridge after build
        EditorPrefs.SetBool("MCPForUnity.DisableDuringBuild", false);
        
        // Restart MCP bridge if needed
        try
        {
            var mcpBridgeType = System.Type.GetType("MCPForUnity.Editor.MCPForUnityBridge");
            if (mcpBridgeType != null)
            {
                var startMethod = mcpBridgeType.GetMethod("Start", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                startMethod?.Invoke(null, null);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[MCP Build Processor] Could not restart MCP bridge: {ex.Message}");
        }
    }
}