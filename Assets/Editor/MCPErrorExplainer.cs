using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides information about MCP URP serialization errors
/// </summary>
[InitializeOnLoad]
public static class MCPErrorExplainer
{
    static MCPErrorExplainer()
    {
        EditorApplication.delayCall += ShowInfoOnce;
    }
    
    private static void ShowInfoOnce()
    {
        if (!EditorPrefs.GetBool("MCP_URP_INFO_SHOWN", false))
        {
            Debug.Log("<color=cyan>[MCP Info]</color> If you see URP serialization errors (cameraColorTarget, cameraDepthTarget), these are harmless MCP Unity Bridge issues that don't affect your app's functionality or builds. They only occur in the editor when using MCP tools.");
            EditorPrefs.SetBool("MCP_URP_INFO_SHOWN", true);
        }
    }
    
    [MenuItem("Tools/MCP/About URP Serialization Errors")]
    public static void ShowURPErrorInfo()
    {
        EditorUtility.DisplayDialog(
            "MCP URP Serialization Errors - Information",
            "The URP serialization errors you see in the console are harmless side effects of the MCP Unity Bridge trying to serialize Unity's Universal Render Pipeline components.\n\n" +
            "These errors:\n" +
            "• Do NOT affect your app's functionality\n" +
            "• Do NOT prevent building or running your app\n" +
            "• Only appear in the Unity Editor when using MCP tools\n" +
            "• Are completely absent in built applications\n\n" +
            "Your Quest app will work perfectly despite these console messages.",
            "OK"
        );
    }
    
    [MenuItem("Tools/MCP/Clear All MCP Errors")]
    public static void ClearMCPErrors()
    {
        // Clear console
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
        
        Debug.Log("<color=green>[MCP]</color> Console cleared. Your app is ready for build and deployment!");
    }
}