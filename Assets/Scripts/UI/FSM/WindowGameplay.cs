using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class WindowGameplay : MAINWindow
{
    public static WindowGameplay Instance;

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

    private SignalBus _signalBus;
    private DataHandler _dataHandler;
    private BackTalkHandler _backTalkHandler;

    private GameObject _parentCombo;
    private PlayerFSM _playerFSM;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        DataHandler dataHandler,
        BackTalkHandler backTalkHandler)
    {
        _signalBus = signalBus;
        _dataHandler = dataHandler;
        _backTalkHandler = backTalkHandler;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<StartBackTalkSignal>(UpdateSubtitle);
        _signalBus.Subscribe<EndBackTalkSignal>(UpdateSubtitle);
        _signalBus.Subscribe<BitUpgradedSignal>(UpdateUI);

        UpdateSubtitle();
    }

    public override void StartWindow()
    {
        Instance = this;
        _parentCombo = _comboLabel.transform.parent.gameObject;
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(Subs());
        _debugTestParam.text = $"{_dataHandler.CurrentData.testSaveParam}";
        //        ControllerDemoSaveFile.Instance.backTalk.OnUpdateTalk += UpdateSubtitle;

        UpdateUI();
    }

    IEnumerator Subs()
    {
        while (PlayerFSM.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _playerFSM = PlayerFSM.Instance;
        _playerFSM.HPComponent.ChangeHP += _panelHP.UpdateHP;
        _playerFSM.OnUpdatePlayer += UpdatePlayerUI;
        _playerFSM.HPComponent.OnChangeHP();
        CancelTarget();
        UpdatePlayerUI();
        _playerFSM.virtualObjectChecker.CheckHints();
    }

    private void UpdatePlayerUI()
    {
        _comboLabel.text = $"Combo:{_playerFSM.Combo}";
        _hitLabel.text = _playerFSM.Hit > 0 ? $"HIT:{_playerFSM.Hit}" : string.Empty;
        _parentCombo.SetActive(_playerFSM.Hit > 0);
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
//            ControllerDemoSaveFile.Instance.backTalk.GetTalk;

        _backTalk.enabled = !string.IsNullOrEmpty(_backTalk.text);
        Color targetColor = _tempColor;
        targetColor.a = _backTalk.enabled ? 1f : 0f;

        DOTween.To(() => _tempColor, x => _backGroundBackTalk.color = x, targetColor, 1f);
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
            if (_playerFSM.GetPoints.EnemyIsTarget)
            {
                CancelTarget();
            }
            else
            {
                TrySetTarget();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.Shift);
        }

        if (_target.enabled)
        {
            _target.rectTransform.position = Camera.main.WorldToScreenPoint(_playerFSM.GetPoints.TargetEnemy.transform.position);
        }

        if (_hintText.enabled && _playerFSM)
        {
            _hintText.rectTransform.position = Camera.main.WorldToScreenPoint(_playerFSM.virtualObjectChecker.LastHH.GetTransform.position);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _signalBus.Fire(new ShowSignal());
        }
    }

    internal void SetHintText(string hint)
    {
        _hintText.text = hint;
        _hintText.enabled = !string.IsNullOrEmpty(hint);
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

    internal void TrySetTarget()
    {
        if (EntityRepository.Instance.HaveEnemies())
        {
            _playerFSM.GetPoints.SetHoldTarget(
                EntityRepository.Instance.GetNearestEnemy(PlayerFSM.Instance.transform.position));
            _target.enabled = true;
        }
        else
        {
            CancelTarget();
        }
    }

    internal void CancelTarget()
    {
        _target.enabled = false;
        _playerFSM.GetPoints.CancelTarget();
    }

    public override void ExitWindow()
    {
        Instance = null;
        base.ExitWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if (_panelHP && _playerFSM)
        {
            _playerFSM.HPComponent.ChangeHP -= _panelHP.UpdateHP;
        }
        if (_playerFSM)
        {
            _playerFSM.OnUpdatePlayer -= UpdatePlayerUI;
        }
//        _dataHandler.CurrentData.BitUpgrade -= UpdateUI;

        _signalBus.Unsubscribe<StartBackTalkSignal>(UpdateSubtitle);
        _signalBus.Unsubscribe<EndBackTalkSignal>(UpdateSubtitle);
        _signalBus.Unsubscribe<BitUpgradedSignal>(UpdateUI);
    }
}