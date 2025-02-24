using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EventByVariant))]
public class EventByVariantDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Получаем поле KeyVariant
        SerializedProperty keyVariantProperty = property.FindPropertyRelative("KeyVariant");

        // Создаем список для выбора
        List<string> keyVariants = GetAllKeyVariants();

        // Если есть доступные ключи, создаем выпадающий список
        if (keyVariants.Count > 0)
        {
            // Найти индекс текущего ключа
            int currentIndex = keyVariants.IndexOf(keyVariantProperty.stringValue);
            // Отрисовываем выпадающий список
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, keyVariants.ToArray());

            // Устанавливаем выбранное значение обратно в поле
            if (selectedIndex >= 0)
            {
                keyVariantProperty.stringValue = keyVariants[selectedIndex];
            }
        }
        else
        {
            // Если нет доступных ключей, отрисуем текст
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

        // Получаем все активные объекты DialogSO
        DialogSO[] dialogObjects = Resources.FindObjectsOfTypeAll<DialogSO>();

        // Пробегаемся по каждому диалогу и добавляем KeyVariant из DialogVariantSO
        foreach (var dialog in dialogObjects)
        {
            foreach (var variant in dialog.dialogSteps) // Предполагается, что у вас есть коллекция dialogVariants
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