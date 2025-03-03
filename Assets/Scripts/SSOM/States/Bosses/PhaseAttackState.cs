using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/PhaseAttackState", order = 1)]
public class PhaseAttackState : State
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private string CustomKey;
    [SerializeField] private float _intervalAttack;

    private EntityFSM _entityFSM;
    private float _localTimer;
    private int level;
    private List<Transform> _points = new();

    internal override string Key => CustomKey;

    protected override void Init()
    {
        base.Init();

        if (Character is EntityFSM entfsm)
        {
            _entityFSM = entfsm;

            if (_entityFSM.GetModule is FuryModule furyModule)
            {
                var phase = furyModule.GetPhase();
                _points = furyModule.GetPoints(this);
                switch (phase)
                {
                    case 0:
                        level = 1;
                        break;
                    case 1:
                        level = 2;
                        break;
                    case 2:
                        level = 4;
                        break;
                    default:
                        break;
                }
                _localTimer = _intervalAttack;
            }
        }
    }

    private void SpawnAndAttack(Transform trns, EnumWhoIs enumWhoIs)
    {
        var w = Instantiate(_weaponPrefab, trns.position, Quaternion.identity);
        w.Init(enumWhoIs,
            trns,
            PlayerFSM.Instance.transform);
    }

    protected override void Run()
    {
        _localTimer -= Time.deltaTime;

        if (_localTimer <= 0)
        {
            if (level > 0)
            {
                SpawnAndAttack(_points[level - 1], _entityFSM.GetArmorVisualizator.GetWhoIs.whoIs);
                level--;
                _localTimer = _intervalAttack;
            }
            else
            {
                IsFinished = true;
            }
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}