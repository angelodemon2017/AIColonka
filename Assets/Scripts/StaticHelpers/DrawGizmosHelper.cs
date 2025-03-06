using UnityEditor;
using UnityEngine;

public static class DrawGizmosHelper
{
    public static void DrawLabel(Transform transform, float high, string name)
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        Handles.Label(transform.position + Vector3.up * high, name, style);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * (high - 1) + Vector3.up * 0.5f);
    }
}