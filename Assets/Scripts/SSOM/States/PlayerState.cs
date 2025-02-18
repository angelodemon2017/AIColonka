using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerState : State
{
    [SerializeField] protected List<PareStateEnumAnim> AvailableControlStates;
    private Dictionary<EnumAnimations, PlayerState> _availableControlStates = new();

    protected PlayerFSM playerFSM;

    protected override void Init()
    {
        AvailableControlStates.ForEach(acs =>
        _availableControlStates.Add(acs.playerAction, acs.playerState));
        playerFSM = Character as PlayerFSM;
        PlayerStateInit();
    }

    protected virtual void PlayerStateInit()
    {

    }

    internal void CallPlayerAction(EnumAnimations playerAction)
    {
        if (_availableControlStates.TryGetValue(playerAction, out PlayerState playerState))
        {
            Character.SetState(playerState);
        }
    }

    internal virtual void CallAxisHorVer(float hor, float ver)
    {

    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }

    [System.Serializable]
    public class PareStateEnumAnim
    {
        public EnumAnimations playerAction;
        public PlayerState playerState;
    }
}