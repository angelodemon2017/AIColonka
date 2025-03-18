using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomSignal))]
public class CustomSignalsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeSignalProperty = property.FindPropertyRelative("typeSignal");
        CustomSignal.TypeSignal typeSignal = (CustomSignal.TypeSignal)typeSignalProperty.enumValueIndex;

        Rect typeSignalRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeSignalRect, typeSignalProperty, new GUIContent("Signal Type"));

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        switch (typeSignal)
        {
            case CustomSignal.TypeSignal.SetTask:
                DrawProperty(position, property.FindPropertyRelative("_setTaskSignal"), "Set Task Signal");
                break;
            case CustomSignal.TypeSignal.BackTalk:
                DrawProperty(position, property.FindPropertyRelative("_backTalkSignal"), "Back Talk Signal");
                break;
            case CustomSignal.TypeSignal.None:
                break;
        }

        EditorGUI.EndProperty();
    }

    private void DrawProperty(Rect position, SerializedProperty property, string label)
    {
        EditorGUI.PropertyField(position, property, new GUIContent(label), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        SerializedProperty typeSignalProperty = property.FindPropertyRelative("typeSignal");
        CustomSignal.TypeSignal typeSignal = (CustomSignal.TypeSignal)typeSignalProperty.enumValueIndex;

        switch (typeSignal)
        {
            case CustomSignal.TypeSignal.SetTask:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_setTaskSignal"), true);
                break;
            case CustomSignal.TypeSignal.BackTalk:
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_backTalkSignal"), true);
                break;
        }

        return height;
    }
}