using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    public bool IsDebug;
    public TaskConfig TaskConfig;
    public EnumLevels CurrentLevel;
    public DialogSO CurrentDialog;

    internal BackTalk backTalk = new BackTalk();

    public MainData mainData = new MainData();

    private void Awake()
    {
        if (!Instance)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    internal TaskSO GetCurrentTask()
    {
        return TaskConfig.GetTaskByKey(mainData.progressHistory.KeyMainTask);
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return TaskConfig.GetTaskByKey(mainData.progressHistory.KeyMainTask) == taskSO;
    }

    internal bool WasDone(TaskSO taskSO)
    {
        return mainData.progressHistory.IsWasDone(taskSO.KeyTitle);
    }

    private void FixedUpdate()
    {
        backTalk.UpdateTime(Time.fixedDeltaTime);
    }

    internal void SetLevel(EnumLevels level)
    {
        CurrentLevel = level;
        mainData.SetLevel(level);
        backTalk.EndTalk();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {

        }
    }
}

public class BackTalk
{
    public string KeyTalk;
    private string LocalText;
    private float _time;

    public Action OnUpdateTalk;
    public Action OnStartTalk;
    public Action OnEndTalk;

    public string GetTalk =>
        string.IsNullOrWhiteSpace(KeyTalk) ? string.Empty :
        LocalText;

    internal async Task SetTalkAsync(string key, float time)
    {
        KeyTalk = key;
        _time = time;
//        OnStartTalk?.Invoke();

        LocalText = await Localizations.GetLocalizedText(
            Localizations.Tables.BackTalksTable,
            KeyTalk);
        OnUpdateTalk?.Invoke();

    }

    internal void UpdateTime(float deltaTime)
    {
        if (_time > 0)
        {
            _time -= deltaTime;
            if (_time <= 0f)
            {
                EndTalk();
            }
        }
    }

    internal void EndTalk()
    {
        KeyTalk = string.Empty;
        _time = 0f;
        LocalText = string.Empty;
        OnUpdateTalk?.Invoke();
        OnEndTalk?.Invoke();
    }
}