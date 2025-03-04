using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/IdleByHPState", order = 1)]
public class IdleByHPState : IdleState
{
    [SerializeField] private AnimationCurve _multTimeByHP;

    protected override float getTime => _multTimeByHP.Evaluate(Character.GetModule.hPComponent.GetPercentHP) * base.getTime;

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}