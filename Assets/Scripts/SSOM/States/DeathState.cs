using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/DeathState", order = 1)]
public class DeathState : State
{
    protected override void Init()
    {
        base.Init();
        if (Character is EntityFSM entityFSM &&
            entityFSM.GetArmorVisualizator.GetWhoIs.whoIs != EnumWhoIs.Player)
        {
            EntityRepository.Instance.RemoveWho(entityFSM.GetArmorVisualizator.GetWhoIs);
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}