using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/GroundAttackState", order = 1)]
public class GroundAttackState : State
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private string CustomKey;

    private EntityFSM _entityFSM;

    internal override string Key => CustomKey;

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
                    furyModule.PointUp,
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