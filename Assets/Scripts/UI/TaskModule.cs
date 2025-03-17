using UnityEngine;
using Zenject;

public class TaskModule : MonoBehaviour
{
    [SerializeField] private TaskPreview _prefabTaskPreview;
    [SerializeField] private Transform _parentTasks;
    [Inject] private TaskConfig _taskConfig;

    internal void Init()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate += UpdateTasks;
        UpdateTasks();
    }

    private void UpdateTasks()
    {
        _parentTasks.DestroyChildrens();
        var newPT = GameObject.Instantiate(_prefabTaskPreview, _parentTasks);

        _ = newPT.InitAsync(_taskConfig.GetTaskByKey(ControllerDemoSaveFile.Instance.mainData.progressHistory.KeyTitleMainTask));
    }

    private void OnDestroy()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= UpdateTasks;
    }
}