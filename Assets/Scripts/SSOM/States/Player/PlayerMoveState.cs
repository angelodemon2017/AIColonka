using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerMoveState", order = 1)]
public class PlayerMoveState : PlayerState
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private EnumAnimations slowMoveAnimate;

    private Transform _avatarTransform;
    private Transform _characterTransform;
    private Transform _cameraTransform;
    private Vector3 desiredMoveDirection;
    private CharacterController _characterController;

    private float _timeOut = 0.1f;

    private float totalSpeed => IsSlowMove ?
        moveSpeed / 2f :
        moveSpeed;
    private bool IsSlowMove => playerFSM.AdditionalStates.IsSlow;
    protected override EnumAnimations GetAnimation =>
        IsSlowMove ?
        slowMoveAnimate :
        Animation;

    protected override void Init()
    {
        base.Init();
        IsFinished = false;
        _characterTransform = Character.GetTransform();
        _characterController = _characterTransform.GetComponent<CharacterController>();
        _avatarTransform = playerFSM.AnimationAdapter.transform;
        _cameraTransform = CameraController.Instance.transform;
//            Camera.main.transform;
    }

/*    internal override void CheckAndUpdateState()
    {
        base.CheckAndUpdateState();
        Character.PlayAnimation(GetAnimation);
    }/**/

    internal override void CallAxisHorVer(float hor, float ver)
    {
        IsFinished = false;
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = (forward * ver + right * hor).normalized;
        _characterController.Move(desiredMoveDirection * totalSpeed * Time.fixedDeltaTime);

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            _avatarTransform.rotation = Quaternion.Slerp(_avatarTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        if (playerFSM.GetFallingController.IsGrounded && playerFSM.GetFallingController.IsFalling)
        {
            playerFSM.GetFallingController.SetYVelocity(-2f);
        }
    }

    protected override void Run()
    {
        base.Run();
    }

    internal override void FixedRun()
    {
        base.FixedRun();

        if (_timeOut <= 0f && _characterController.velocity == Vector3.zero)
        {
            IsFinished = true;
        }

        _timeOut -= Time.fixedDeltaTime;
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}