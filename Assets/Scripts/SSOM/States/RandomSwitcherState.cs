using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/RandomSwitcherState", order = 1)]
public class RandomSwitcherState : State
{
    [SerializeField] private List<State> _randomStates;

    protected override void Init()
    {
        base.Init();
        if (_randomStates.Count > 0)
        {
            Character.SetState(_randomStates.GetRandom());
            IsFinished = true;
        }
        else
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}