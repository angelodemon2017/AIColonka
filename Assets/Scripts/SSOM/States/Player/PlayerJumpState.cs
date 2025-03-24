using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerJumpState", order = 1)]
public class PlayerJumpState : PlayerState
{
//    [SerializeField] private PlayerState _fixPlatformState;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _rotationSpeed;
    private float gravity = -9.81f;

    protected override void Init()
    {
        base.Init();
        if (_playerFSM.GetFallingController.IsGrounded)
        {
            _playerFSM.GetFallingController.SetYVelocity(Mathf.Sqrt(jumpHeight * -2f * gravity));
        }
        else
        {
            IsFinished = true;
        }
    }

    internal override void FixedRun()
    {
        if (_playerFSM.GetFallingController.IsGrounded)
        {
//            IsFinished = true;
        }/**/
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        MovePlayer(hor, ver, _horizontalSpeed, _rotationSpeed);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}