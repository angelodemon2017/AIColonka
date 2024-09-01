using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SSOController : MonoBehaviour, IStatesCharacter, IMovableCharacter
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private ColliderController _colliderController;

    [SerializeField] private Transform _keepObjectPoint;
    [SerializeField] private State _startingState;
    private State _currentState;

    private List<Property> _props = new();

    public bool IsFinishedCurrentState() => _currentState.IsFinished;

    public Transform GetTransform() => transform;
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;

    private void Awake()
    {
        if (_colliderController)
        {
            _colliderController.ColliderAction += AddProp;
        }
        if (_animationAdapter)
        {
            _animationAdapter.triggerPropAction += AddProp;
        }
        SetState(_startingState);
    }

    private void Update()
    {
        _currentState.RunState();
    }

    private void LateUpdate()
    {
        ClearProps();
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

        _currentState?.ExitState();

        _currentState = Instantiate(state);
        _currentState.InitState(this);
    }

    public void GoToPoint(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    private void AddProp(EnumProps prop)
    {
        if (_props.Any(x => x.Prop == prop))
        {
            return;
        }

        _props.Add(new Property(prop));
    }

    public bool CheckProp(EnumProps prop)
    {
        var pr = _props.FirstOrDefault(x => x.Prop == prop);
        if (pr != null)
        {
            pr.Check = true;
        }
        return pr != null;
    }

    private void ClearProps()
    {
        List<Property> tempProps = new();

        foreach (var p in _props)
        {
            if (p.Check)
            {
                tempProps.Add(p);
            }
            else
            {
                p.Check = true;
            }
        }

        foreach (var p in tempProps)
        {
            _props.Remove(p);
        }
    }

    public void TakeObject(GameObject keepObj)
    {
        keepObj.transform.SetParent(_keepObjectPoint);
        keepObj.transform.position = _keepObjectPoint.position;
    }
}