using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/Levels/PlayerDirectState", order = 1)]
public class PlayerDirectMoveState : PlayerState
{
    [SerializeField] private Vector3 _directSpeed;
    [SerializeField] private Vector3 _rightDirect;
    [SerializeField] private float _speed;

    private Vector3 _sideSpeed = Vector3.zero;
    private CharacterController _characterController;

    protected override void Init()
    {
        base.Init();
        _characterController = Character.GetTransform().GetComponent<CharacterController>();
    playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(_directSpeed);
    }

    internal override void FixedRun()
    {
        base.FixedRun();
        _characterController.Move((_directSpeed + _sideSpeed) * Time.fixedDeltaTime);
        _sideSpeed = Vector3.zero;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        _sideSpeed = hor > 0 ? _rightDirect : _rightDirect * -1;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}