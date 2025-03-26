using DG.Tweening;
using UnityEngine;
using Zenject;

public class UIFSM : MonoBehaviour
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
            SetWindowSignal(new SetWindowSignal(_startWindow));
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
        if (setWindowSignal.SelectWindow.NeedTransition)
        {
            _signalBus.Fire(new TransitionSignal());
        }
        DOTween.To(() => 1f, x => TransitionWindow(x), 0f, 0.5f)
            .OnComplete(() => OpenWindow(setWindowSignal.SelectWindow));
    }

    internal void OpenWindow(MAINWindow windowFSM)
    {
        _canvasGroup.interactable = false;
        if (_currentWindow != null)
        {
            _currentWindow.ExitWindow();
        }
        //place for pool
        _parent.DestroyChildrens();

        _currentWindow = Instantiate(windowFSM, _parent);
            //_container.InstantiatePrefabForComponent<MAINWindow>(windowFSM, _parent);

        _container.Inject(_currentWindow);

        StartWindow();

        _canvasGroup.interactable = true;
    }

    private void TransitionWindow(float progress)
    {
        _canvasGroup.alpha = progress;
    }

    private void StartWindow()
    {
        if (_currentWindow != null)
        {
            _currentWindow.StartWindow();
            DOTween.To(() => 0f, x => TransitionWindow(x), 1f, 0.5f);
            if (_currentWindow.NeedTransition)
            {
                _signalBus.Fire(new TransitionSignal(true));
            }
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