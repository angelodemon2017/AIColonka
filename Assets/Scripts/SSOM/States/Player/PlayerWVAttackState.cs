using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerWVAttackState", order = 1)]
public class PlayerWVAttackState : PlayerState, IWVState
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private string _customKey;

    private FallingController _fallingController;

    internal bool IsAir => !_playerFSM.GetFallingController.IsGrounded;

    public int GetCountLaunched => 1;
    public float GetIntervalLaunched => 0.1f;
    public Weapon GetWeapon => _weapon;
    internal override string Key => _customKey;

    protected override void Init()
    {
        base.Init();

        _fallingController = _playerFSM.GetFallingController;

        if (IsAir && !_fallingController.AvailableActionInAir)
        {
            IsFinished = true;
            return;
        }

        if (_playerFSM.WeaponVisualizator)
        {
            _fallingController.SwitchGravity();

            _playerFSM.WeaponVisualizator.CallAttack(this);

            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            _playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(forward.normalized);
        }
        else
        {
            Finish();
        }
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