using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/AirAttackState", order = 1)]
public class AirAttackState : State
{
    [SerializeField] private Weapon _weaponPrefab;

    private EntityFSM _entityFSM;

    protected override void Init()
    {
        base.Init();

        if (Character is EntityFSM entfsm)
        {
            _entityFSM = entfsm;

            if (_entityFSM.GetModule is FuryModule furyModule)
            {
                var w = Instantiate(_weaponPrefab, furyModule.PointDown.position, Quaternion.identity);
                w.Init(entfsm.GetArmorVisualizator.GetWhoIs.whoIs,
                    furyModule.PointDown,
                    PlayerFSM.Instance.transform);
            }
        }
        IsFinished = true;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}