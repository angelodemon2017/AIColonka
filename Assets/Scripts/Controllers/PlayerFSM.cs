using UnityEngine;

public class PlayerFSM : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private PlayerState _startState;

    private PlayerState _currentState;
    private Transform _transform;
    private Vector3 _velocity;
    private bool _isGrounded;

    internal Vector3 Velocity => _velocity;
    internal bool IsGrounded => _isGrounded;
    internal AnimationAdapter AnimationAdapter => _animationAdapter;
    public Transform GetTransform() => _transform;
    public bool IsFinishedCurrentState() => _currentState.IsFinished;

    private void Awake()
    {
        _transform = transform;
        SetState(_startState);
    }

    internal void SetYVelocity(float y)
    {
        _velocity.y = y;
    }

    private void Update()
    {
        _currentState.RunState();
    }

    internal void CallPlayerAction(EnumAnimations playerAction)
    {
        _currentState.CallPlayerAction(playerAction);
    }

    internal void CallAxisHorVer(float hor, float ver)
    {
//        if (hor != 0 || ver != 0)
//        {
            _currentState.CallAxisHorVer(hor, ver);
//        }
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
}