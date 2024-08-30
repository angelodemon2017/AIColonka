using UnityEngine;

[CreateAssetMenu]
public class IdleState : State
{
    public override void Init(IStatesCharacter character)
    {
        base.Init(character);
        IsFinished = false;
    }

    public override void Run()
    {
        if (IsFinished)
        {

            return;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return false;
    }
}