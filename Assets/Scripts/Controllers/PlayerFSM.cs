using System;
using UnityEngine;

public class PlayerFSM : MonoBehaviour, IStatesCharacter
{
    public static PlayerFSM Instance;

    [SerializeField] private VirtualObjectChecker _virtualObjectChecker;
    [SerializeField] private AdditionalStates _additionalStates;
    [SerializeField] private BitsController _bitsController;
    [SerializeField] private int _combo;
    [SerializeField] private int _hit;
    [SerializeField] private Points _points;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private FallingController _fallingController;

    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private PlayerState _startState;

    [SerializeField] private PlayerState _currentState;

    private Transform _transform;
    private float _hitUpdate;

    internal Action OnUpdatePlayer;

    #region properties
    internal AdditionalStates AdditionalStates => _additionalStates;
    internal ArmorVisualizator GetArmorVisualizator => _armorVisualizator;
    internal AnimationAdapter AnimationAdapter => _animationAdapter;
    public Transform GetTransform() => _transform;
    public bool IsFinishedCurrentState() => _currentState.IsFinished;
    internal FallingController GetFallingController => _fallingController;
    internal HPComponent HPComponent => _hpComponent;
    internal Transform PointOfTargetForEnemy => _points.PointOfTargetForEnemy;
    internal Points GetPoints => _points;
    internal BitsController BitsController => _bitsController;
    internal VirtualObjectChecker virtualObjectChecker => _virtualObjectChecker;
    internal int Combo 
    {
        get => _combo;
        set 
        {
            _combo = value;
            OnUpdatePlayer?.Invoke();
        }
    }
    internal int Hit 
    {
        get => _hit;
        set 
        {
            _hit = value;
            if (_hit > 0)
            {
                _hitUpdate = 3f;
            }
            OnUpdatePlayer?.Invoke();
        }
    }

    public EntityModule GetModule => null;
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
        _armorVisualizator.SetPoints(_points);
    }

    internal void SetSlowState(bool isSlow)
    {
        _additionalStates.SetSlow(isSlow);
//        _currentState.CheckAndUpdateState();
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
        if (_hitUpdate > 0f)
        {
            _hitUpdate -= Time.fixedDeltaTime;
            if (_hitUpdate <= 0f)
            {
                Hit = 0;
            }
        }
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

    [ContextMenu("CallAVA")]
    private void CallAV()
    {
        CallPlayerAction(EnumPlayerControlActions.AVAttack);
    }
}