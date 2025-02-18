using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/DeathState", order = 1)]
public class DeathState : State
{
    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}