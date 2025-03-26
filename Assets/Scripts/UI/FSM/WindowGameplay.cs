using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using System.Threading.Tasks;

public class WindowGameplay : MAINWindow
{
    [SerializeField] private TextMeshProUGUI _hintText;
    [SerializeField] private TaskModule _taskModule;
    [SerializeField] private PanelHP _panelHP;
    [SerializeField] private TextMeshProUGUI _debugTestParam;
    [SerializeField] private TextMeshProUGUI _bitLabel;
    [SerializeField] private TextMeshProUGUI _avwPowerLabel;
    [SerializeField] private Image _target;

    [SerializeField] private TextMeshProUGUI _comboLabel;
    [SerializeField] private TextMeshProUGUI _hitLabel;

    [SerializeField] private Color _tempColor;
    [SerializeField] private Image _backGroundBackTalk;
    [SerializeField] private TextMeshProUGUI _backTalk;

    [SerializeField] private Image _backGroundTaskNotification;
    [SerializeField] private TextMeshProUGUI _taskNotification;

    private CameraController _cameraController;
    private DataHandler _dataHandler;
    private BackTalkHandler _backTalkHandler;
    private GameplayHandler _gameplayHandler;

    private GameObject _parentCombo;
    private PlayerFSM _playerFSM;

    [Inject]
    private void Construct(
        CameraController cameraController,
        DataHandler dataHandler,
        BackTalkHandler backTalkHandler,
        GameplayHandler gameplayHandler)
    {
        _cameraController = cameraController;
        _dataHandler = dataHandler;
        _backTalkHandler = backTalkHandler;
        _gameplayHandler = gameplayHandler;

        Init();
    }

    private void Init()
    {        
        _signalBus.Subscribe<BitUpgradedSignal>(UpdateUI);
        _signalBus.Subscribe<EndBackTalkSignal>(UpdateSubtitle);
        _signalBus.Subscribe<FocusHintSignal>(UpdateHintText);
        _signalBus.Subscribe<MetaFightSignal>(UpdateFightMetaUI);
        _signalBus.Subscribe<StartBackTalkSignal>(UpdateSubtitle);
        _signalBus.Subscribe<TaskUpdatedSignal>(UpdateTaskNotificationAsync);
        _signalBus.Subscribe<WhoInTargetSignal>(UpdateTargetUI);

        UpdateSubtitle();
        UpdateTaskNotificationAsync();
    }

    public override void StartWindow()
    {
        _parentCombo = _comboLabel.transform.parent.gameObject;
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _debugTestParam.text = $"{_dataHandler.CurrentData.testSaveParam}";

        UpdateTargetUI();
        UpdateUI();
        InitPlayer();
    }

    private void InitPlayer()
    {
        _playerFSM = _gameplayHandler.PlayerInstance;
        _playerFSM.HPComponent.ChangeHP += _panelHP.UpdateHP;
        _playerFSM.HPComponent.OnChangeHP();
        UpdateFightMetaUI();
        _playerFSM.virtualObjectChecker.CheckHints();
    }

    private void UpdateFightMetaUI()
    {
        _comboLabel.text = $"Combo:{_gameplayHandler.Combo}";
        _hitLabel.text = _gameplayHandler.Hit > 0 ? $"HIT:{_gameplayHandler.Hit}" : string.Empty;
        _parentCombo.SetActive(_gameplayHandler.Hit > 0);
    }

    private void UpdateUI()
    {
        _bitLabel.text =
            _dataHandler.CurrentData.gamePlayProgress.BattleBits > 0 ?
            $"B:{_dataHandler.CurrentData.gamePlayProgress.BattleBits}" :
            string.Empty;

        _avwPowerLabel.text =
            _dataHandler.CurrentData.gamePlayProgress.AVPower > 0 ?
            $"AVP:{_dataHandler.CurrentData.gamePlayProgress.AVPower}" :
            string.Empty;
    }

    private void UpdateSubtitle()
    {
        _backTalk.text = _backTalkHandler.GetTalk;

        _backTalk.enabled = !string.IsNullOrEmpty(_backTalk.text);
        Color targetColor = _tempColor;
        targetColor.a = _backTalk.enabled ? 1f : 0f;

        DOTween.To(() => _tempColor, x => _backGroundBackTalk.color = x, targetColor, 1f);
    }

    private void UpdateTaskNotificationAsync()
    {
        var curNewTask = _dataHandler.GetNotifTask();
        if (curNewTask)
        {
            _ = UpdateTaskTextNotification(curNewTask);
            DOTween.To(() => Color.clear, x => _backGroundTaskNotification.color = x, Color.black, 1f)
                .OnComplete(() =>
                {
                    DOTween.To(() => Color.black, x => _backGroundTaskNotification.color = x, Color.clear, 1f)
                        .SetDelay(1f)
                        .OnComplete(() => _taskNotification.text = string.Empty);
                });
        }
    }

    private async Task UpdateTaskTextNotification(TaskSO curNewTask)
    {
        _taskNotification.text = await curNewTask.GetTitle();
    }

    public override void Run()
    {
        base.Run();

        if (Input.GetButtonDown("Jump"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.Jump);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.BladeAttack);
        }

        if (Input.GetButtonDown("Fire2") && _dataHandler.CurrentData.gamePlayProgress.BattleBits > 0)
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.BitAttack);
        }

        if (Input.GetButtonDown("Fire3") && _dataHandler.CurrentData.gamePlayProgress.AVPower >= 0)
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.AVAttack);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _playerFSM.CallTryRelease();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _gameplayHandler.UpdateTarget();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.Shift);
        }

        if (_target.enabled)
        {
            _target.rectTransform.position =
                _cameraController.CurrentCamera.WorldToScreenPoint(
                    _gameplayHandler.InTarget.transform.position);
        }

        if (_hintText.enabled && _playerFSM)
        {
            _hintText.rectTransform.position =
                _cameraController.CurrentCamera.WorldToScreenPoint(
                    _playerFSM.virtualObjectChecker.LastHH.GetTransform.position);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
        if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    internal void UpdateHintText()
    {
        _hintText.text = _gameplayHandler.FocusHint;
        _hintText.enabled = !string.IsNullOrEmpty(_gameplayHandler.FocusHint);
    }

    public override void FixedRun()
    {
        base.FixedRun();

        var mX = Input.GetAxis("Mouse X");
        var mY = Input.GetAxis("Mouse Y");

        if (mX != 0 || mY != 0)
        {
            _playerFSM?.MoveMouse(mX, mY);
        }

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            _playerFSM.CallAxisHorVer(horizontal, vertical);
        }
    }

    private void UpdateTargetUI()
    {
        _target.enabled = _gameplayHandler.InTarget;
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if (_panelHP && _playerFSM)
        {
            _playerFSM.HPComponent.ChangeHP -= _panelHP.UpdateHP;
        }

        _signalBus.Unsubscribe<BitUpgradedSignal>(UpdateUI);
        _signalBus.Unsubscribe<EndBackTalkSignal>(UpdateSubtitle);
        _signalBus.Unsubscribe<FocusHintSignal>(UpdateHintText);
        _signalBus.Unsubscribe<MetaFightSignal>(UpdateFightMetaUI);
        _signalBus.Unsubscribe<StartBackTalkSignal>(UpdateSubtitle);
        _signalBus.Unsubscribe<TaskUpdatedSignal>(UpdateTaskNotificationAsync);
        _signalBus.Unsubscribe<WhoInTargetSignal>(UpdateTargetUI);
    }
}