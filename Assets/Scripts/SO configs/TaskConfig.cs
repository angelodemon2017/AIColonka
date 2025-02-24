using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TaskConfig", order = 1)]
public class TaskConfig : ScriptableObject
{
    public List<TaskSO> MainTasks;
    [SerializeField] private TaskSO _missTask;

    public TaskSO GetTaskByKey(string key)
    {
        return MainTasks.FirstOrDefault(t => t.Key == key);
    }

    public TaskSO GetTaskById(int id)
    {
        return id < MainTasks.Count ? MainTasks[id] : _missTask;
    }
}