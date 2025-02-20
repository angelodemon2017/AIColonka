using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerBladeAttackState", order = 1)]
public class PlayerBladeAttackState : PlayerState
{
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private Damage _damage;
    [SerializeField] private float _size;

    private EnumPlayerControlActions _lastAction = EnumPlayerControlActions.None;

    protected override void Init()
    {
        base.Init();

        var az = Instantiate(_attackZone);
        az.SetDamage(_damage);
        az.SetAttackZone(_size);
        az.Init(EnumWhoIs.Player);
        az.transform.position = playerFSM.GetPoints.PointAttackZone1.position;
    }

    internal override void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _lastAction = playerAction;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {

    }

    internal override void EndCurrentAnimation(float timeEnd = 0)
    {
        base.EndCurrentAnimation(timeEnd);

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