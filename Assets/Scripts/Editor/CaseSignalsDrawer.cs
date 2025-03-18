using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomPropertyDrawer(typeof(CaseSignals))]
public class CaseSignalsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeSignalProperty = property.FindPropertyRelative("typeSignal");
        Rect typeSignalRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeSignalRect, typeSignalProperty, new GUIContent("Signal Type"));

        CaseSignals.TypeSignal typeSignal = (CaseSignals.TypeSignal)typeSignalProperty.enumValueIndex;

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        DrawSignalFields(position, property, typeSignal);

        EditorGUI.EndProperty();
    }

    private void DrawSignalFields(Rect position, SerializedProperty property, CaseSignals.TypeSignal typeSignal)
    {
        FieldInfo[] fields = typeof(CaseSignals).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            SignalTypeAttribute attribute = field.GetCustomAttribute<SignalTypeAttribute>();

            if (attribute != null && attribute.TypeSignal == typeSignal)
            {
                SerializedProperty signalProperty = property.FindPropertyRelative(field.Name);

                if (signalProperty != null)
                {
                    Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(signalProperty, true));
                    EditorGUI.PropertyField(fieldRect, signalProperty, new GUIContent(ObjectNames.NicifyVariableName(field.Name)), true);
                    position.y += fieldRect.height + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        SerializedProperty typeSignalProperty = property.FindPropertyRelative("typeSignal");
        CaseSignals.TypeSignal typeSignal = (CaseSignals.TypeSignal)typeSignalProperty.enumValueIndex;

        FieldInfo[] fields = typeof(CaseSignals).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            SignalTypeAttribute attribute = field.GetCustomAttribute<SignalTypeAttribute>();

            if (attribute != null && attribute.TypeSignal == typeSignal)
            {
                SerializedProperty signalProperty = property.FindPropertyRelative(field.Name);
                if (signalProperty != null)
                {
                    height += EditorGUI.GetPropertyHeight(signalProperty, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        return height;
    }
}