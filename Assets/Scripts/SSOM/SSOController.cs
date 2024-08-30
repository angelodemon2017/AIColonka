using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SSOController : MonoBehaviour, IStatesCharacter, IMovableCharacter
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private List<State> _triggeredStates = new();

    [SerializeField] private State _startingState;
    private State _currentState;


    public bool IsFinishedCurrentState() => _currentState.IsFinished;

    public Transform GetTransform() => transform;

    private void Awake()
    {
        _startingState = _triggeredStates[0];
        SetState(_startingState);
    }

    private void Update()
    {
        _currentState.RunState();
    }

    public void PlayAnimation(EnumAnimations animation)
    {
        _animationAdapter.PlayAnimationEvent(animation);
    }

    public void SetState(State state)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState.ExitState();

        _currentState = Instantiate(state);
        _currentState.Character = this;
        _currentState.InitState(this);
    }

    public void GoToPoint(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }
}