using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerState : State
{
    [SerializeField] protected List<PareStateEnumAnim> AvailableControlStates;
    protected Dictionary<EnumPlayerControlActions, PlayerState> _availableControlStates = new();

    protected Transform _avatarTransform;
    protected PlayerFSM _playerFSM;
    protected Transform _cameraTransform;

    [Inject]
    protected CameraController _cameraController;

    internal virtual bool IsBladeAttack => false;

    protected override void Init()
    {
        AvailableControlStates.ForEach(acs =>
        _availableControlStates.Add(acs.playerAction, acs.playerState));
        _playerFSM = Character as PlayerFSM;
        _avatarTransform = _playerFSM.AnimationAdapter.transform;
        _cameraTransform = _cameraController.transform;
    }

    internal virtual void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        if (_availableControlStates.TryGetValue(playerAction, out PlayerState playerState))
        {
            Character.SetState(playerState);
        }
    }

    internal virtual void CallAxisHorVer(float hor, float ver) { }

    private Vector3 desiredMoveDirection;
    protected virtual void MovePlayer(float hor, float ver, float speed, float rotSpeed)
    {
        Vector3 forward = Camera.main.transform.forward;
//            _cameraTransform.forward;
        Vector3 right = Camera.main.transform.right;
        //_cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = (forward * ver + right * hor).normalized;
        var totalMove = desiredMoveDirection * speed * Time.fixedDeltaTime;
        _playerFSM.MovePerson(totalMove);

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            _avatarTransform.rotation = Quaternion.Slerp(_avatarTransform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
        }
    }

    internal virtual void EndCurrentAnimation(float timeEnd) 
    {
        if (timeEnd <= 0f)
        {
            SetFinished();
        }
    }

    protected virtual void SetFinished()
    {
        IsFinished = true;
    }

    protected override void Run()
    {
        base.Run();

        _playerFSM.AnimationAdapter.PlayAnimationEvent(GetAnimation);
    }

    internal override void FixedRun()
    {
        base.FixedRun();
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }

    [System.Serializable]
    public class PareStateEnumAnim
    {
        public EnumPlayerControlActions playerAction;
        public PlayerState playerState;
    }
}