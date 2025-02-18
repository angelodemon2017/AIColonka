using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    [SerializeField] protected List<PareStateEnumAnim> AvailableControlStates;
    protected Dictionary<EnumAnimations, PlayerState> _availableControlStates = new();

    protected PlayerFSM playerFSM;

    protected override void Init()
    {
        AvailableControlStates.ForEach(acs =>
        _availableControlStates.Add(acs.playerAction, acs.playerState));
        playerFSM = Character as PlayerFSM;
    }

    internal virtual void CallPlayerAction(EnumAnimations playerAction)
    {
        if (_availableControlStates.TryGetValue(playerAction, out PlayerState playerState))
        {
            Character.SetState(playerState);
        }
    }

    internal virtual void CallAxisHorVer(float hor, float ver) { }

    internal virtual void EndCurrentAnimation() { }

    protected override void Run()
    {
        base.Run();

        playerFSM.AnimationAdapter.PlayAnimationEvent(Animation);
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