using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/SelectorStateByPhaseState", order = 1)]
public class SelectorStateByPhaseState : State
{
    [SerializeField] private List<State> _stateByPhase;

    protected override void Init()
    {
        if (Character.GetModule is IPhaselable phase)
        {
            Character.SetState(_stateByPhase.GetBorderElement(phase.GetPhase()));
        }
        else
        {
            Finish();
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}