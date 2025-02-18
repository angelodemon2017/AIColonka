using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/TakeDamageState", order = 1)]
public class TakeDamageState : State
{
    protected override void Run()
    {
        if (Character.CheckProp(EnumProps.EndAnimate))
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.CheckProp(EnumProps.TakingDamage);
    }
}