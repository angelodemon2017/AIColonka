using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerAVAttackState", order = 1)]
public class PlayerAVAttackState : PlayerState
{
    private ArmorVisualizator _armorVisualizator;
    private FallingController _fallingController;

    private float _timeOut = 0.75f;

    internal bool IsAir => !playerFSM.GetFallingController.IsGrounded;

    protected override void Init()
    {
        base.Init();

        _fallingController = playerFSM.GetFallingController;

        if (IsAir && !_fallingController.AvailableActionInAir)
        {
            IsFinished = true;
            return;
        }

        _fallingController.SwitchGravity();

        _armorVisualizator = playerFSM.GetArmorVisualizator;
        _armorVisualizator.CallAttack(GetTypeAttack());

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;

        playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(forward.normalized);
    }

    private ArmorVisualizator.TypeVisualAttack GetTypeAttack()
    {
        return ArmorVisualizator.TypeVisualAttack.Middle;
    }

    protected override void Run()
    {
        base.Run();

        _timeOut -= Time.deltaTime;
        if (_timeOut <= 0f)
        {
            IsFinished = true;
        }
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {

    }

    public override void ExitState()
    {
        base.ExitState();

        _fallingController.ResetFalling();
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}