using System.Collections;
using UnityEngine;
using TMPro;

public class WindowGameplay : MAINWindow
{
    [SerializeField] private TaskController _taskController = new();
    [SerializeField] private PanelHP _panelHP;
    [SerializeField] private TextMeshProUGUI _debugTestParam;

    private PlayerFSM _playerFSM;

    public override void StartWindow()
    {
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _taskController.Init();
        StartCoroutine(Subs());
        _debugTestParam.text = $"{ControllerDemoSaveFile.Instance.mainData.testSaveParam}";
    }

    IEnumerator Subs()
    {
        while (PlayerFSM.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _playerFSM = PlayerFSM.Instance;
        _playerFSM.HPComponent.ChangeHP += _panelHP.UpdateHP;
        _playerFSM.HPComponent.OnChangeHP();
    }

    public override void Run()
    {
        base.Run();

        /*        CameraController.Instance.UpdateMouse(
                    Input.GetAxis("Mouse X"),
                    Input.GetAxis("Mouse Y"));/**/

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

        if (Input.GetButtonDown("Jump"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.Jump);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.BladeAttack);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.BitAttack);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            _playerFSM.CallPlayerAction(EnumPlayerControlActions.AVAttack);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (CameraController.Instance.EnemyInTarget)
            {
                CancelTarget();
            }
            else if (EntityRepository.Instance.HaveEnemies())
            {
                CameraController.Instance.SetEnemyTarget(
                    EntityRepository.Instance.GetNearestEnemy(PlayerFSM.Instance.transform.position));
            }
        }
    }

    internal void CancelTarget()
    {
        CameraController.Instance.CancelEnemyTarget();
    }

    public override void ExitWindow()
    {
        _taskController.Deatcivate();
        base.ExitWindow();
//        PersonMovement.Instance.OnMovePlayer(0, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        _playerFSM.HPComponent.ChangeHP -= _panelHP.UpdateHP;
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

        newPT.Init(ControllerDemoSaveFile.Instance.GetCurrentTask());
    }

    internal void Deatcivate()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= UpdateTasks;
    }
}