using UnityEngine;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    public TaskConfig TaskConfig;

    public DialogSO CurrentDialog;

    public int testParam;
    public MainData mainData = new MainData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
//        Debug.Log($"Awake ControllerDemoSaveFile");
    }

    private void Start()
    {
//        Debug.Log($"Start ControllerDemoSaveFile");
    }

    internal TaskSO GetCurrentTask()
    {
        return TaskConfig.GetTaskById(mainData.progressHistory.CurrentTask);
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return TaskConfig.GetTaskById(mainData.progressHistory.CurrentTask) == taskSO;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            testParam++;
        }
    }
}