using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    [SerializeField] private float _speedTransl;
    [SerializeField] private Image _blackImage;
    [SerializeField] private TextMeshProUGUI _testLoading;
    public bool IsDebug;
    public TaskConfig TaskConfig;
    public EnumLevels CurrentLevel;
    public DialogSO CurrentDialog;

    private Color _transColor;
    private bool IsTranslate = false;
    internal BackTalk backTalk = new BackTalk();

    public MainData mainData = new MainData();

    private float _totalSpeed => //IsDebug ? 1 : 
        _speedTransl;
    public bool IsBlackEnd => _blackImage.color.a >= 1f;

    private void Awake()
    {
        if (!Instance)
        {
            _transColor = _blackImage.color;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            SceneLevelLoader.LoadProgress += UpdateLoading;
        }
    }

    internal TaskSO GetCurrentTask()
    {
        return TaskConfig.GetTaskByKey(mainData.progressHistory.KeyTitleMainTask);
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

    private void FixedUpdate()
    {
        backTalk.UpdateTime(Time.fixedDeltaTime);
        CheckTransl();
    }

    internal void SetLevel(EnumLevels level)
    {
        SetBlack(true);
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

    private void UpdateLoading(float progres)
    {
        _testLoading.text = progres == 0 ? string.Empty : $"Load:{progres}";
    }

    private void CheckTransl()
    {
        if (IsTranslate && _blackImage.color.a < 1)
        {
            _transColor.a += _totalSpeed;
            _blackImage.color = _transColor;
        }
        else if (!IsTranslate && _blackImage.color.a > 0)
        {
            _transColor.a -= _totalSpeed;
            _blackImage.color = _transColor;
        }
    }

    internal void SetBlack(bool isOn)
    {
        IsTranslate = isOn;
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

    internal async Task SetTalkAsync(string key, float time, string fromLocalTable)
    {
        KeyTalk = key;
        _time = time;

        LocalText = await Localizations.GetLocalizedText(
            fromLocalTable, KeyTalk);

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