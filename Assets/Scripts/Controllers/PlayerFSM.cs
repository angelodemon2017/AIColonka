using UnityEngine;

public class PlayerFSM : MonoBehaviour, IStatesCharacter
{
    public static PlayerFSM Instance;

    [SerializeField] private BitsController _bitsController;
    [SerializeField] private int _combo;
    [SerializeField] private Points _points;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private FallingController _fallingController;

    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private PlayerState _startState;

    [SerializeField] private PlayerState _currentState;
    private Transform _transform;

    #region properties

    internal ArmorVisualizator GetArmorVisualizator => _armorVisualizator;
    internal AnimationAdapter AnimationAdapter => _animationAdapter;
    public Transform GetTransform() => _transform;
    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    internal FallingController GetFallingController => _fallingController;
    internal HPComponent HPComponent => _hpComponent;
    internal Transform PointOfTargetForEnemy => _points.PointOfTargetForEnemy;
    internal Points GetPoints => _points;
    internal BitsController BitsController => _bitsController;
    internal int Combo 
    {
        get => _combo;
        set 
        {
            _combo = value;
        }
    }
    #endregion

    private void Awake()
    {
        Instance = this;
        _transform = transform;
        _animationAdapter.EndAnimation += EndCurrentAnimate;
        SetState(_startState);
        CameraController.Instance.SetPivot(
            _points.PointOfMoveCamera,
            _points.PointOfLookCamera);
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
        _currentState.EndCurrentAnimation();
    }

    internal void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _currentState.CallPlayerAction(playerAction);
    }

    internal void CallAxisHorVer(float hor, float ver)
    {
        _currentState.CallAxisHorVer(hor, ver);
    }

    public void SetState(State state)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = Instantiate(state as PlayerState);
        _currentState.InitState(this);
    }

    public bool CheckProp(EnumProps prop) => true;

    public void InitAttackZone(GameObject attackZone) { }

    public void PlayAnimation(EnumAnimations animation) { }

    public void TakeObject(GameObject keepObj) { }

    private void OnDestroy()
    {
        Instance = null;
        _animationAdapter.EndAnimation -= EndCurrentAnimate;
    }
}