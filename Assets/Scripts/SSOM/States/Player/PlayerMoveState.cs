using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerMoveState", order = 1)]
public class PlayerMoveState : PlayerState
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private FallingController _fallingController;
    private float _timeOut = 0.1f;

    protected override void Init()
    {
        base.Init();
        IsFinished = false;
        _fallingController = _playerFSM.GetFallingController;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        IsFinished = false;

        MovePlayer(hor, ver, moveSpeed, rotationSpeed);

        _timeOut = 0.1f;

        if (_fallingController.IsGrounded && _fallingController.IsFalling)
        {
            _fallingController.SetYVelocity(-2f);
        }
    }

    protected override void Run()
    {
        base.Run();
    }

    internal override void FixedRun()
    {
        base.FixedRun();

        if (_timeOut <= 0f)// && playerFSM.FinalMagnitude < totalSpeed)
        {
            IsFinished = true;
        }

        _timeOut -= Time.fixedDeltaTime;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}