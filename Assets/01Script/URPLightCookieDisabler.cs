#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class URPLightCookieDisabler : EditorWindow
{
    [MenuItem("Tools/Disable URP Light Cookies")]
    public static void DisableLightCookies()
    {
        var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (urpAsset == null)
        {
            Debug.LogWarning("URP Asset not found in Graphics Settings.");
            return;
        }

        var so = new SerializedObject(urpAsset);
        var prop = so.FindProperty("m_SupportsLightCookies");

        if (prop != null)
        {
            prop.boolValue = false;
            so.ApplyModifiedProperties();
            Debug.Log("Light Cookies disabled on current URP Asset.");
        }
        else
        {
            Debug.LogWarning("Light Cookie property not found (property may be hidden or internal).");
        }
    }
}
#endif