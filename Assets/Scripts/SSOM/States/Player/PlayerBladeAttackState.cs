using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerBladeAttackState", order = 1)]
public class PlayerBladeAttackState : PlayerState
{
    private EnumPlayerControlActions _lastAction = EnumPlayerControlActions.None;

    internal override void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _lastAction = playerAction;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {

    }

    internal override void EndCurrentAnimation()
    {
        base.EndCurrentAnimation();

        if (_availableControlStates.TryGetValue(_lastAction, out PlayerState playerState))
        {
            Character.SetState(playerState);
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