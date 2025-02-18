using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerFallingState", order = 1)]
public class PlayerFallingState : PlayerState
{
    internal override void CallAxisHorVer(float hor, float ver) 
    {
        //correct move in fall
    }

    protected override void Run()
    {
        base.Run();

        if (playerFSM.GetFallingController.IsGrounded)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        if (character is PlayerFSM playerFSM)
        {
            return !playerFSM.GetFallingController.IsGrounded && playerFSM.GetFallingController.IsFalling;
        
        }
        return false;
    }
}