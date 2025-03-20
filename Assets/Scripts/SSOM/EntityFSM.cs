using UnityEngine;
using Zenject;

public class EntityFSM : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private EnumAirGroundState _airGroundState;
    [SerializeField] private WhoIs _whoIs;
    [SerializeField] private PanelHP _UIpanelHP;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private State _startState;
    [SerializeField] private EntityModule _entityModule;

    private DiContainer _diContainer;
    private GameplayHandler _gameplayHandler;
    public State _currentState;

    internal WhoIs whoIs => _whoIs;
    public EntityModule GetModule => _entityModule;
    internal EnumAirGroundState airGroundState => _airGroundState;

    public DiContainer Container => _diContainer;

    [Inject]
    private void Construct(
        DiContainer diContainer,
        GameplayHandler gameplayHandler)
    {
        _diContainer = diContainer;
        _gameplayHandler = gameplayHandler;
    }

    private void Awake()
    {
        _hpComponent.ChangeHP += _UIpanelHP.UpdateHP;
        _hpComponent.OnChangeHP();

        _entityModule?.Init();

        SetState(_startState);
    }

    private void Start()
    {
        _gameplayHandler.AddWho(_whoIs);
    }

    private void Update()
    {
        _currentState.RunState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedRun();
    }

    public void SetState(State state)
    {
        SetState(state, false);
    }

    internal void SetAGS(EnumAirGroundState airGroundState)
    {
        _airGroundState = airGroundState;
    }

    public void SetState(State state, bool ignoreEqual = false)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = Instantiate(state);
        _currentState.InitState(this);
    }

    private void OnDestroy()
    {
        _hpComponent.ChangeHP -= _UIpanelHP.UpdateHP;
    }

    public bool IsFinishedCurrentState()
    {
        return _currentState.IsFinished;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void PlayAnimation(EnumAnimations animation) { }
}