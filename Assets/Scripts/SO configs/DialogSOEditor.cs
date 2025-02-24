using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(DialogSO))]
public class DialogSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogSO dialogSO = (DialogSO)target;

        // Отображаем стандартные поля
        DrawDefaultInspector();

        // Проверяем, есть ли элементы в списке dialogVariants
        if (dialogSO.dialogSteps.Count > 0)
        {
            foreach (var variant in dialogSO.dialogSteps)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Dialog Variant", EditorStyles.boldLabel);

                // Создаем массив строк для выпадающего списка
                string[] options = new string[dialogSO.dialogSteps.Count];
                for (int i = 0; i < dialogSO.dialogSteps.Count; i++)
                {
                    options[i] = dialogSO.dialogSteps[i].KeyPersonTextV0; // Заполняем массив значениями KeyPersonTextV0
                }

                // Находим индекс текущего IdStepDialog
                int currentIndex = -1;
                for (int i = 0; i < dialogSO.dialogSteps.Count; i++)
                {
                    if (dialogSO.dialogSteps[i].IdStep == variant.IdStep)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                // Создаем выпадающий список
                int selectedIndex = EditorGUILayout.Popup("IdStepDialog2", currentIndex, options);

                // Если выбранный индекс не равен -1, обновляем IdStepDialog
                if (selectedIndex != -1)
                {
                    variant.IdStep = dialogSO.dialogSteps[selectedIndex].IdStep; // Обновляем IdStepDialog
                }

                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No dialog steps available.");
        }
    }
}/**/