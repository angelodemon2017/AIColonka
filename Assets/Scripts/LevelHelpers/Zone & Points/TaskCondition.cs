using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class TaskCondition : MonoBehaviour
{
    [SerializeField] private TaskSO _task;
    [SerializeField] private bool __ifIsFuture;
    [SerializeField] private bool __ifIsCurrent;
    [SerializeField] private bool __ifIsCompleted;

    private SignalBus _signalBus;
    private DataHandler _dataHandler;

    private string IfDone => __ifIsCompleted ? "D" : string.Empty;
    private string IfCurrent => __ifIsCurrent ? "C" : string.Empty;
    private string IfFuture => __ifIsFuture ? "F" : string.Empty;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        DataHandler dataHandler)
    {
        _signalBus = signalBus;
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<TaskUpdatedSignal>(CheckTask);

        CheckTask();
    }

    private void Awake()
    {
//        CheckTask();
    }

    private void CheckTask()
    {
        if (_dataHandler.IsCurrentTask(_task))
        {
            gameObject.SetActive(__ifIsCurrent);
        }        
        else if (_dataHandler.WasDone(_task))
        {
            gameObject.SetActive(__ifIsCompleted);
        }
        else
        {            
            gameObject.SetActive(__ifIsFuture);
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 2f, $"TaskCondition:{(_task == null ? "NO SELECT" : _task.name)}({IfDone}{IfCurrent}{IfFuture})");
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<TaskUpdatedSignal>(CheckTask);
    }
}