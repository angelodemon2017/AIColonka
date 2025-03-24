using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerIdleState", order = 1)]
public class PlayerIdleState : PlayerState
{
    [SerializeField] private PlayerMoveState _playerMoveState;

    private bool _moveAvailable = true;

    protected override void Init()
    {
        base.Init();
        _moveAvailable = _playerMoveState;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        if (_moveAvailable)
        {
            Character.SetState(_playerMoveState);
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}