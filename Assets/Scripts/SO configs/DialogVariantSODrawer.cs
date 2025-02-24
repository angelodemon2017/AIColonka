using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DialogVariantSO))]
public class DialogVariantSODrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Получаем доступ к родительскому объекту DialogSO
        DialogSO dialogSO = property.serializedObject.targetObject as DialogSO;

        if (dialogSO != null && dialogSO.dialogSteps.Count > 0)
        {
            // Создаем массив строк для выпадающего списка
            string[] options = new string[dialogSO.dialogSteps.Count];
            for (int i = 0; i < dialogSO.dialogSteps.Count; i++)
            {
                options[i] = dialogSO.dialogSteps[i].KeyPersonTextV0; // Заполняем массив значениями KeyPersonTextV0
            }

            // Находим текущее значение IdStepDialog
            int currentIndex = -1;
            for (int i = 0; i < dialogSO.dialogSteps.Count; i++)
            {
                if (dialogSO.dialogSteps[i].IdStep == property.FindPropertyRelative("IdStepDialog2").intValue)
                {
                    currentIndex = i;
                    break;
                }
            }

            // Отрисовка дополнительных полей
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("KeyVariant"), new GUIContent("Key Variant"));
            position.y += EditorGUIUtility.singleLineHeight + 2; // Сдвигаем позицию вниз для следующего поля
//            EditorGUI.PropertyField(position, property.FindPropertyRelative("specEndDialog"), new GUIContent("Spec End Dialog"));
//            position.y += EditorGUIUtility.singleLineHeight + 2; // Сдвигаем позицию вниз для следующего поля

            // Создаем выпадающий список
            int selectedIndex = EditorGUI.Popup(position, "Select Step", currentIndex, options);

            // Если выбранный индекс не равен -1, обновляем IdStepDialog
            if (selectedIndex != -1)
            {
                property.FindPropertyRelative("IdStepDialog2").intValue = dialogSO.dialogSteps[selectedIndex].IdStep; // Обновляем IdStepDialog
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "No dialog steps available.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Высота для двух дополнительных полей и выпадающего списка
        return EditorGUIUtility.singleLineHeight * 2 + 4; // 3 строки + отступы
    }
}