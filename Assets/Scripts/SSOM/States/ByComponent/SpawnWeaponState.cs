using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/SpawnWeaponState", order = 1)]
public class SpawnWeaponState : State
{


    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}