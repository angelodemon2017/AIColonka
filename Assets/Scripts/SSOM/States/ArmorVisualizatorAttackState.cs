using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/ArmorVisualizatorAttackState", order = 1)]
public class ArmorVisualizatorAttackState : State
{
    private ArmorVisualizator _armorVisualizator;

    protected override void Init()
    {
        if (Character is EntityFSM entityFSM)
        {
/*            _armorVisualizator = entityFSM.GetArmorVisualizator;
            _armorVisualizator.SetTarget(PlayerFSM.Instance.PointOfTargetForEnemy);
            _armorVisualizator.CallAttack(ArmorVisualizator.TypeVisualAttack.Middle);/**/
        }
        else
        {
            IsFinished = true;
        }
    }

    protected override void Run()
    {
        if (Character.CheckProp(EnumProps.EndAnimate))
        {
            Debug.Log("AttackState isFinished");
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}