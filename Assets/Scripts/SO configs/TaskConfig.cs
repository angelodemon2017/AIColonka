using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "SO/TaskConfig", order = 1)]
public class TaskConfig : ScriptableObjectInstaller<TaskConfig>
{
    [SerializeField] private TaskSO _missTask;
    public CacheList<string, TaskSO> _mainTasks;

    private void OnValidate()
    {
        for (int i = 0; i < _mainTasks.Count(); i++)
        {
            _mainTasks[i].KeyTitle = $"MT{i}";
            _mainTasks[i].KeyLocDesc = $"MD{i}";
        }
    }

    public TaskSO GetTaskByKey(string key)
    {
        return _mainTasks.GetByKey(key);
    }

    public override void InstallBindings()
    {
        Container.BindInstance(this);
    }
}