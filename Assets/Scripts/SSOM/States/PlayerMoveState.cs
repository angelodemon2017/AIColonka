using UnityEngine;

[CreateAssetMenu]
public class PlayerMoveState : PlayerState
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Transform _characterTransform;
    private Transform _cameraTransform;
    private Vector3 desiredMoveDirection;
    private CharacterController _characterController;

    protected override void PlayerStateInit()
    {
        base.PlayerStateInit();
        _cameraTransform = Camera.main.transform;
        _characterTransform = Character.GetTransform();
        _characterController = _characterTransform.GetComponent<CharacterController>();
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = (forward * ver + right * hor).normalized;
        _characterController.Move(desiredMoveDirection * moveSpeed);

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            _characterTransform.rotation = Quaternion.Slerp(_characterTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (playerFSM.IsGrounded && playerFSM.Velocity.y < 0)
        {
            playerFSM.SetYVelocity(-2f);
        }
    }

    protected override void Run()
    {
        base.Run();

        playerFSM.AnimationAdapter.PlayAnimationEvent(Animation);

        if (playerFSM.Velocity == Vector3.zero)
        {
//            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}