using UnityEngine;
using Zenject;

public class TaskModule : MonoBehaviour
{
    [SerializeField] private TaskPreview _prefabTaskPreview;
    [SerializeField] private Transform _parentTasks;

    private SignalBus _signalBus;
    private TaskConfig _taskConfig;
    private DataHandler _dataHandler;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        TaskConfig taskConfig,
        DataHandler dataHandler)
    {
        _signalBus = signalBus;
        _taskConfig = taskConfig;
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<TaskUpdatedSignal>(UpdateTasks);
        UpdateTasks();
    }

    private void UpdateTasks()
    {
        _parentTasks.DestroyChildrens();
        var newPT = GameObject.Instantiate(_prefabTaskPreview, _parentTasks);

        _ = newPT.InitAsync(_taskConfig.GetTaskByKey(_dataHandler.CurrentData.progressHistory.KeyTitleMainTask));
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<TaskUpdatedSignal>(UpdateTasks);
    }
}