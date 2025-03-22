using UnityEngine;
using Zenject;

public class UIFSM : MonoBehaviour, IUIFSM
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private MAINWindow _startWindow;

    public GameplayHandler DebugDATA;

    private Transform _parent;

    private IWindowFSM _currentWindow;

    private SignalBus _signalBus;
    private DiContainer _container;
    private SceneController _sceneController;

    [Inject]
    private void Construct(
        GameplayHandler gameplayHandler,
        DiContainer container,
        SignalBus signalBus,
        SceneController sceneController)
    {
        DebugDATA = gameplayHandler;
        _container = container;
        _signalBus = signalBus;
        _sceneController = sceneController;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<SetLevelSignal>(LoadLevel);
        _signalBus.Subscribe<SetWindowSignal>(SetWindowSignal);

        _parent = transform;

        if (_startWindow)
        {
            OpenWindow(_startWindow);
        }
    }

    private void Update()
    {
        _currentWindow?.Run();
    }

    private void FixedUpdate()
    {
        _currentWindow?.FixedRun();
    }

    private void LoadLevel(SetLevelSignal setLevelSignal)
    {
        _sceneController.LoadLevelByEnum(setLevelSignal.Level);
    }

    private void SetWindowSignal(SetWindowSignal setWindowSignal)
    {
        OpenWindow(setWindowSignal.SelectWindow);
    }

    public MAINWindow OpenWindow(MAINWindow windowFSM)
    {
        if (_currentWindow != null)
        {
            _currentWindow.ExitWindow();
        }
        //place for pool
        _parent.DestroyChildrens();

        _currentWindow = _container.InstantiatePrefabForComponent<MAINWindow>(windowFSM, _parent);
        StartWindow();

        return _currentWindow as MAINWindow;
    }

    private void StartWindow()
    {
        if (_currentWindow != null)
        {
            _currentWindow.StartWindow(); 
        }
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SetWindowSignal>(SetWindowSignal);
        _signalBus.Unsubscribe<SetLevelSignal>(LoadLevel);
        if (_currentWindow != null)
        {
            _currentWindow.ExitWindow();
        }
    }
}