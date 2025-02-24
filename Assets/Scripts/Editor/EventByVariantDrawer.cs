using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EventByVariant))]
public class EventByVariantDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �������� ���� KeyVariant
        SerializedProperty keyVariantProperty = property.FindPropertyRelative("KeyVariant");

        // ������� ������ ��� ������
        List<string> keyVariants = GetAllKeyVariants();

        // ���� ���� ��������� �����, ������� ���������� ������
        if (keyVariants.Count > 0)
        {
            // ����� ������ �������� �����
            int currentIndex = keyVariants.IndexOf(keyVariantProperty.stringValue);
            // ������������ ���������� ������
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, keyVariants.ToArray());

            // ������������� ��������� �������� ������� � ����
            if (selectedIndex >= 0)
            {
                keyVariantProperty.stringValue = keyVariants[selectedIndex];
            }
        }
        else
        {
            // ���� ��� ��������� ������, �������� �����
            EditorGUI.LabelField(position, label.text, "No KeyVariants available.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    private List<string> GetAllKeyVariants()
    {
        List<string> keyVariants = new List<string>();

        // �������� ��� �������� ������� DialogSO
        DialogSO[] dialogObjects = Resources.FindObjectsOfTypeAll<DialogSO>();

        // ����������� �� ������� ������� � ��������� KeyVariant �� DialogVariantSO
        foreach (var dialog in dialogObjects)
        {
            foreach (var variant in dialog.dialogSteps) // ��������������, ��� � ��� ���� ��������� dialogVariants
            {
                if (!keyVariants.Contains(variant.KeyPersonTextV0))
                {
                    keyVariants.Add(variant.KeyPersonTextV0);
                }
            }
        }

        return keyVariants;
    }
}