using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WindowGameplay : MAINWindow
{
    public static WindowGameplay Instance;

    [SerializeField] private TextMeshProUGUI _hintText;
    [SerializeField] private TaskController _taskController = new();
    [SerializeField] private PanelHP _panelHP;
    [SerializeField] private TextMeshProUGUI _debugTestParam;
    [SerializeField] private TextMeshProUGUI _bitLabel;
    [SerializeField] private TextMeshProUGUI _avwPowerLabel;
    [SerializeField] private Image _target;

    [SerializeField] private TextMeshProUGUI _comboLabel;
    [SerializeField] private TextMeshProUGUI _hitLabel;

    [SerializeField] private TextMeshProUGUI _backTalk;

    private PlayerFSM _playerFSM;

    private MainData _mainData => ControllerDemoSaveFile.Instance.mainData;

    public override void StartWindow()
    {
        Instance = this;
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _taskController.Init();
        StartCoroutine(Subs());
        _debugTestParam.text = $"{_mainData.testSaveParam}";

        ControllerDemoSaveFile.Instance.backTalk.OnUpdateTalk += UpdateSubtitle;
        _mainData.BitUpgrade += UpdateUI;

        UpdateUI();
        UpdateSubtitle();
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
    }

    private void UpdateUI()
    {
        _bitLabel.text =
            _mainData.gamePlayProgress.BattleBits > 0 ?
            $"B:{_mainData.gamePlayProgress.BattleBits}" :
            string.Empty;

        _avwPowerLabel.text =
            _mainData.gamePlayProgress.AVPower > 0 ?
            $"AVP:{_mainData.gamePlayProgress.AVPower}" :
            string.Empty;
    }

    private void UpdateSubtitle()
    {
        _backTalk.text = ControllerDemoSaveFile.Instance.backTalk.GetTalk;
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

        if (Input.GetButtonDown("Fire2") && ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits > 0)
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.BitAttack);
        }

        if (Input.GetButtonDown("Fire3") && ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.AVPower >= 0)
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

        if (_target.enabled)
        {
            _target.rectTransform.position = Camera.main.WorldToScreenPoint(_playerFSM.GetPoints.TargetEnemy.transform.position);
        }

        if (_hintText.enabled)
        {
            _hintText.rectTransform.position = Camera.main.WorldToScreenPoint(_playerFSM.virtualObjectChecker.LastHH.GetTransform.position);
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
            _playerFSM?.GetPoints.Move(mX, mY);
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
        _taskController.Deatcivate();
        base.ExitWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        _playerFSM.HPComponent.ChangeHP -= _panelHP.UpdateHP;
        _playerFSM.OnUpdatePlayer -= UpdatePlayerUI;
        _mainData.BitUpgrade -= UpdateUI;
        ControllerDemoSaveFile.Instance.backTalk.OnUpdateTalk -= UpdateSubtitle;
    }
}

[System.Serializable]
public class TaskController
{
    [SerializeField] private TaskPreview _prefabTaskPreview;
    [SerializeField] private Transform _parentTasks;

    internal void Init()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate += UpdateTasks;
        UpdateTasks();
    }

    private void UpdateTasks()
    {
        _parentTasks.DestroyChildrens();
        var newPT = GameObject.Instantiate(_prefabTaskPreview, _parentTasks);

        _ = newPT.InitAsync(ControllerDemoSaveFile.Instance.GetCurrentTask());
    }

    internal void Deatcivate()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= UpdateTasks;
    }
}