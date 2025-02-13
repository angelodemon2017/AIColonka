using UnityEngine;

public class WindowGameplay : MAINWindow
{
    [SerializeField] private TaskController _taskController = new();

    public override void StartWindow()
    {
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _taskController.Init();
//        CameraController.Instance.ResetCamera();
    }

    public override void Run()
    {
        base.Run();

        CameraController.Instance.UpdateMouse(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        PersonMovement.Instance.OnMovePlayer(horizontal, vertical);

        if (Input.GetButtonDown("Jump"))
        {
            PersonMovement.Instance.OnJump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PersonMovement.Instance.OnAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            PersonMovement.Instance.OnSecondAttack();
        }
    }

    public override void ExitWindow()
    {
        _taskController.Deatcivate();
        base.ExitWindow();
        PersonMovement.Instance.OnMovePlayer(0, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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