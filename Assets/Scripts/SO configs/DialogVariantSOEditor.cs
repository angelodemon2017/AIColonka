using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(DialogVariantSO))]
public class DialogVariantSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Получаем ссылку на объект DialogVariantSO
        DialogVariantSO dialogVariantSO = (DialogVariantSO)target;

        // Получаем ссылку на родительский объект DialogSO
        DialogSO dialogSO = FindObjectOfType<DialogSO>();

        // Отображаем стандартные поля
        DrawDefaultInspector();

        // Проверяем, есть ли элементы в списке dialogSteps
        if (dialogSO != null && dialogSO.dialogSteps.Count > 0)
        {
            // Создаем массив строк для выпадающего списка
            string[] options = new string[dialogSO.dialogSteps.Count];
            for (int i = 0; i < dialogSO.dialogSteps.Count; i++)
            {
                options[i] = dialogSO.dialogSteps[i].KeyPersonTextV0; // Заполняем массив значениями KeyPersonTextV0
            }

            // Создаем выпадающий список
            int selectedIndex = EditorGUILayout.Popup("IdStepDialog2", dialogVariantSO.IdStepDialog, options);

            // Если выбранный индекс не равен -1, обновляем IdStepDialog
            if (selectedIndex != -1)
            {
                dialogVariantSO.IdStepDialog = dialogSO.dialogSteps[selectedIndex].IdStep; // Обновляем IdStepDialog
            }
        }
        else
        {
            EditorGUILayout.LabelField("No dialog steps available.");
        }
    }
}/**/