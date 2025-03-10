using UnityEngine;
using UnityEngine.Events;

public class TaskCondition : MonoBehaviour
{
    [SerializeField] private TaskSO _task;
    [SerializeField] private bool __ifIsFuture;
    [SerializeField] private bool __ifIsCurrent;
    [SerializeField] private bool __ifIsCompleted;

    private string IfDone => __ifIsCompleted ? "D" : string.Empty;
    private string IfCurrent => __ifIsCurrent ? "C" : string.Empty;
    private string IfFuture => __ifIsFuture ? "F" : string.Empty;

    private void Awake()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate += CheckTask;
        CheckTask();
    }

    private void CheckTask()
    {
        if (ControllerDemoSaveFile.Instance.WasDone(_task))
        {
            gameObject.SetActive(__ifIsCompleted);
        }
        else if (ControllerDemoSaveFile.Instance.IsCurrentTask(_task))
        {
            gameObject.SetActive(__ifIsCurrent);
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
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= CheckTask;
    }
}