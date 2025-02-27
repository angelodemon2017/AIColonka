using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerIdleState", order = 1)]
public class PlayerIdleState : PlayerState
{
    [SerializeField] private PlayerMoveState _playerMoveState;

    internal override void CallAxisHorVer(float hor, float ver)
    {
        Character.SetState(_playerMoveState);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}