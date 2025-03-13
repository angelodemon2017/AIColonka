using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    [SerializeField] private Image _blackImage;
    [SerializeField] private TextMeshProUGUI _testLoading;
    [SerializeField] private TaskConfig _taskConfig;
    //    public TaskConfig TaskConfig;
    public EnumLevels CurrentLevel;
    public DialogSO CurrentDialog;

    private Color _transColor;
    internal BackTalk backTalk = new BackTalk();

    public MainData mainData = new MainData();
    [SerializeField] private Settings _settings = new Settings();

    internal Settings Settings => _settings;
    public bool IsDebug => _settings.IsDebug;

    public bool IsBlackEnd => _blackImage.color.a >= 1f;


    [Inject]
    public void Construct(TaskConfig taskConfig)
    {
        _taskConfig = taskConfig;
    }

    private void Awake()
    {
        if (!Instance)
        {
            _transColor = _blackImage.color;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            _settings = SaveController.Load<Settings>(Settings.Prefix);
            SceneLevelLoader.LoadProgress += UpdateLoading;
        }
    }

    internal TaskSO GetCurrentTask()
    {
        Debug.Log("!!!GetCurrentTask");
        return _taskConfig.GetTaskByKey(mainData.progressHistory.KeyTitleMainTask);
    }

    internal void SetTask(TaskSO taskSO)
    {
        mainData.SetTask(taskSO);
        _ = backTalk.SetTalkAsync(taskSO.KeyTitle, 2f, Localizations.Tables.Tasks);
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return mainData.progressHistory.KeyTitleMainTask == taskSO.KeyTitle;
    }

    internal bool WasDone(TaskSO taskSO)
    {
        return mainData.progressHistory.IsWasDone(taskSO.KeyTitle);
    }

    internal void SetLevel(EnumLevels level)
    {
        SetBlack(true);
        CurrentLevel = level;
        if (level != EnumLevels.MainMenu)
        {
            mainData.SetLevel(level);
        }
        backTalk.EndTalk();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {

        }
    }

    private void UpdateLoading(float progres)
    {
        _testLoading.text = progres == 0 ? string.Empty : $"Load:{progres}";
    }

    internal void SetBlack(bool fadeIn)
    {
        Color targetColor = _transColor;
        targetColor.a = fadeIn ? 1f : 0f;

        DOTween.To(() => _transColor, x => _blackImage.color = x, targetColor, 1f);
    }
}

public class BackTalk
{
    public string KeyTalk;
    private string LocalText;

    public Action OnUpdateTalk;
    public Action OnStartTalk;
    public Action OnEndTalk;

    public string GetTalk =>
        string.IsNullOrWhiteSpace(KeyTalk) ? string.Empty :
        LocalText;

    internal async Task SetTalkAsync(string key, float time, string fromLocalTable)
    {
        KeyTalk = key;

        DOVirtual.DelayedCall(time, EndTalk);
        LocalText = await Localizations.GetLocalizedText(
            fromLocalTable, KeyTalk);

        OnUpdateTalk?.Invoke();
    }

    internal void EndTalk()
    {
        KeyTalk = string.Empty;
        LocalText = string.Empty;
        OnUpdateTalk?.Invoke();
        OnEndTalk?.Invoke();
    }
}