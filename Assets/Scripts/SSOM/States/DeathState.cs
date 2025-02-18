using UnityEngine;

[CreateAssetMenu]
public class DeathState : State
{
    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}