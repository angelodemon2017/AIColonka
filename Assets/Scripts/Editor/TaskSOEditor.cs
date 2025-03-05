using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

[CustomEditor(typeof(TaskSO))]
public class TaskSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TaskSO taskSO = (TaskSO)target;

        // Отображаем стандартные поля
        DrawDefaultInspector();

        // Отображаем локализованную строку по KeyLocDesc
        if (!string.IsNullOrEmpty(taskSO.KeyLocDesc))
        {
//            _ = UpdateLabelAsync(taskSO.KeyLocDesc);
        }

        // Добавляем кнопку
        if (GUILayout.Button("Update Keys"))
        {
            taskSO.UpdateLocKeys();
//            AddKeysToLocalizationTable(taskSO.KeyTitle, taskSO.KeyLocDesc, taskSO._locTable);
            EditorUtility.SetDirty(taskSO);
        }
    }

    private async Task UpdateLabelAsync(string key)
    {
        string localisedValue = await Localizations.GetLocalizedText(
                        Localizations.Tables.Tasks, key);
        EditorGUILayout.LabelField("Label:", localisedValue);
    }
}