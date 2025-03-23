using UnityEngine;
using Zenject;

public class PlayerFSM : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private VirtualObjectChecker _virtualObjectChecker;
    [SerializeField] private WeaponVisualizator _weaponVisualizator;
    [SerializeField] private BitsController _bitsController;
    [SerializeField] private Points _points;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private FallingController _fallingController;

    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private PlayerState _startState;

    [SerializeField] private PlayerState _currentState;

    [SerializeField] private Transform platform;

    private Transform _transform;
    
    #region properties
    public DiContainer Container => _diContainer;
    internal CharacterController CharacterController => _characterController;
    internal WeaponVisualizator WeaponVisualizator => _weaponVisualizator;
    internal AnimationAdapter AnimationAdapter => _animationAdapter;
    public Transform GetTransform() => _transform;
    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    internal FallingController GetFallingController => _fallingController;
    internal HPComponent HPComponent => _hpComponent;
    internal Transform PointOfTargetForEnemy => _points.PointOfTargetForEnemy;
    internal Points GetPoints => _points;
    internal BitsController BitsController => _bitsController;
    internal VirtualObjectChecker virtualObjectChecker => _virtualObjectChecker;

    public EntityModule GetModule => null;
    #endregion

    private DiContainer _diContainer;
    private DataHandler _dataHandler;
    private CameraController _cameraController;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        DiContainer diContainer,
        SignalBus signalBus,
        CameraController cameraController,
        DataHandler dataHandler)
    {
        _diContainer = diContainer;
        _dataHandler = dataHandler;
        _signalBus = signalBus;

        _cameraController = cameraController;

        _diContainer.Inject(_points);

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<SetPlayerStateSignal>(SetPlayerStateSignal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            platform = other.transform;
            UpdateParent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == platform)
        {
            platform = null;
            UpdateParent();
        }
    }

    private void UpdateParent()
    {
        transform.SetParent(platform?.parent);
    }

    private void Awake()
    {
        _transform = transform;
        _animationAdapter.EndAnimation += EndCurrentAnimate;

        _cameraController.SetPivot(
            _points.PointOfMoveCamera,
            _points.PointOfLookCamera);

        var chap = _dataHandler.CurrentData.chapter;
        _hpComponent.OverrideStats(chap.MaxHP, chap.HPRegenBySecond);

        SetState(_startState);
    }

    internal void CallTryRelease()
    {
        _virtualObjectChecker.CallRelease();
    }

    private void Update()
    {
        _currentState.RunState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedRun();
        _points.FixUpd();
    }

    private void EndCurrentAnimate()
    {
        if (_currentState.IsBladeAttack)
        {
            _currentState.EndCurrentAnimation(0f);
        }
    }

    internal void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _currentState.CallPlayerAction(playerAction);
    }

    internal void MoveMouse(float mX, float mY)
    {
        if (_currentState is PlayerDashState)
        {
            return;
        }

        _points.Move(mX, mY);
    }

    internal void CallAxisHorVer(float hor, float ver)
    {
        _currentState.CallAxisHorVer(hor, ver);
    }

    public void SetState(State state, bool ignoreEqual = false)
    {
        if (_currentState == state && !ignoreEqual)
        {
            return;
        }

        _currentState?.ExitState();

        var nextState = Instantiate(state) as PlayerState;

        SetPreparedState(nextState);
    }

    private void SetPlayerStateSignal(SetPlayerStateSignal signal)
    {
        var nextState = Instantiate(signal._playerState);

        SetPreparedState(nextState);
    }

    internal void SetPreparedState(PlayerState state)
    {
        _currentState = state;
        _currentState.InitState(this);
    }

    public void PlayAnimation(EnumAnimations animation) { }

    private void OnDestroy()
    {
        _animationAdapter.EndAnimation -= EndCurrentAnimate;
    }

    [ContextMenu("CallJump")]
    private void CallAV()
    {
        CallPlayerAction(EnumPlayerControlActions.Jump);
    }
}