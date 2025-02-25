using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TaskConfig", order = 1)]
public class TaskConfig : ScriptableObject
{
    public List<TaskSO> MainTasks;
    [SerializeField] private TaskSO _missTask;

    private void OnValidate()
    {
        for (int i = 0; i < MainTasks.Count; i++)
        {
            MainTasks[i].KeyTitle = $"MT{i}";
            MainTasks[i].KeyLocDesc = $"MD{i}";
        }
    }

    public TaskSO GetTaskByKey(string key)
    {
        return MainTasks.FirstOrDefault(t => t.KeyTitle == key);
    }

    public TaskSO GetTaskById(int id)
    {
        return id < MainTasks.Count ? MainTasks[id] : _missTask;
    }
}