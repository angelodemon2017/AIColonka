using UnityEngine;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    public bool IsDebug;
    public TaskConfig TaskConfig;
    public EnumLevels CurrentLevel;
    public DialogSO CurrentDialog;

    public MainData mainData = new MainData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
//        Debug.Log($"Awake ControllerDemoSaveFile");
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"XText:{(int)mainData.levelsState.LevelProps}");
        }
    }
}