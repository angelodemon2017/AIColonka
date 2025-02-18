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
        while (PersonMovement.Instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        PersonMovement.Instance.GetHPComponent.ChangeHP += _panelHP.UpdateHP;
        PersonMovement.Instance.GetHPComponent.OnChangeHP();

        _playerFSM = PersonMovement.Instance.GetComponent<PlayerFSM>();
    }

    public override void Run()
    {
        base.Run();

        CameraController.Instance.UpdateMouse(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        //        PersonMovement.Instance.OnMovePlayer(horizontal, vertical);

        if (horizontal != 0 || vertical != 0)
        {
            _playerFSM.CallAxisHorVer(horizontal, vertical);
        }

        if (Input.GetButtonDown("Jump"))
        {
//            PersonMovement.Instance.OnJump();
            _playerFSM.CallPlayerAction(EnumAnimations.jump);
        }

        if (Input.GetButtonDown("Fire1"))
        {
//            PersonMovement.Instance.OnAttack();
            _playerFSM.CallPlayerAction(EnumAnimations.attack1);
        }

        if (Input.GetButtonDown("Fire2"))
        {
//            PersonMovement.Instance.OnSecondAttack();
            _playerFSM.CallPlayerAction(EnumAnimations.attack2);
        }

        if (Input.GetButtonDown("Fire3"))
        {
//            PersonMovement.Instance.CallArmor();
            _playerFSM.CallPlayerAction(EnumAnimations.attack3);
        }
    }

    public override void ExitWindow()
    {
        _taskController.Deatcivate();
        base.ExitWindow();
        PersonMovement.Instance.OnMovePlayer(0, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        PersonMovement.Instance.GetHPComponent.ChangeHP -= _panelHP.UpdateHP;
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