using System.Collections.Generic;
using UnityEngine;

public class SSOController : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private List<State> _triggeredStates = new();

    [SerializeField] private State _startingState;
    private State _currentState;

    private void Awake()
    {
        _startingState = _triggeredStates[0];
        SetState(_startingState);
    }

    private void Update()
    {
/*        foreach (var tempState in _triggeredStates)
        {
            if (tempState.GeneralTrigger)
            {
                SetState(tempState);
            }
        }/**/

        _currentState.Run();
/*        if (_currentState.IsResetState)
        {
            SetState(_startingState);
        }/**/
    }

    public void PlayAnimation()//Animates idAnim)
    {

    }

    public void SetState(State state)
    {
        _currentState = Instantiate(state);
        _currentState.Character = this;
        _currentState.Init(this);
    }
}