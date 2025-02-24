using UnityEngine;
using UnityEngine.Events;

public class TaskCondition : MonoBehaviour
{
    [SerializeField] private TaskSO _task;
    [SerializeField] private UnityEvent _ifIsFuture;
    [SerializeField] private UnityEvent _ifIsCurrent;
    [SerializeField] private UnityEvent _ifIsCompleted;

    private void Awake()
    {
        CheckTask();
    }

    private void CheckTask()
    {
        if (ControllerDemoSaveFile.Instance.WasDone(_task))
        {
            _ifIsCompleted?.Invoke();
        }
        else if (ControllerDemoSaveFile.Instance.IsCurrentTask(_task))
        {
            _ifIsCurrent?.Invoke();
        }
        else
        {
            _ifIsFuture?.Invoke();
        }
    }
}