using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerJumpState", order = 1)]
public class PlayerJumpState : PlayerState
{
    [SerializeField] private float jumpHeight;
    private float gravity = -9.81f;

    protected override void Init()
    {
        base.Init();
        if (playerFSM.GetFallingController.IsGrounded)
        {
            playerFSM.GetFallingController.SetYVelocity(Mathf.Sqrt(jumpHeight * -2f * gravity));
        }
        else
        {
            IsFinished = true;
        }
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        //correct jump
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}