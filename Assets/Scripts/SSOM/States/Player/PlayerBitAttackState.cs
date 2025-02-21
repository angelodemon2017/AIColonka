using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerBitAttackState", order = 1)]
public class PlayerBitAttackState : PlayerState
{
    [SerializeField] private List<BitWeapon> _bitWeapons = new();

    private FallingController _fallingController;

    private EnumPlayerControlActions _lastAction = EnumPlayerControlActions.None;

    private float _timeOut = 0f;

    private bool _nextAttackIsBit => _lastAction == EnumPlayerControlActions.BitAttack;
    internal bool IsAir => !playerFSM.GetFallingController.IsGrounded;

    protected override void Init()
    {
        base.Init();

        _fallingController = playerFSM.GetFallingController;

        if (IsAir && !_fallingController.AvailableActionInAir)// || 
//            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits <= 0)
        {
            IsFinished = true;
            return;
        }

        _fallingController.SwitchGravity();

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(forward);

        playerFSM.BitsController.SetBits(false);

        InitBitWeapon();
    }

    private void InitBitWeapon()
    {
        var w = Instantiate(_bitWeapons[Mathf.Clamp(playerFSM.Combo, 0, _bitWeapons.Count - 1)]);

        w.SetPBAS(this, playerFSM.GetPoints);
        w.Init(EnumWhoIs.Player,
            playerFSM.GetPoints.PointOfLookCamera,
            playerFSM.GetPoints.EnemyIsTarget ?
                playerFSM.GetPoints.TargetEnemy.transform :
                null,
            Camera.main.transform.rotation);
    }

    internal override void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _lastAction = playerAction;
    }

    protected override void Run()
    {
        base.Run();

        if (_timeOut > 0f)
        {
            _timeOut -= Time.deltaTime;
            if (_timeOut <= 0f)
            {
                SetFinished();
            }
        }
    }

    protected override void SetFinished()
    {
        if (_availableControlStates.TryGetValue(_lastAction, out PlayerState playerState))
        {
            Character.SetState(playerState, _nextAttackIsBit);
        }
        else
        {
            base.SetFinished();
        }
    }

    internal override void EndCurrentAnimation(float timeEnd)
    {
        base.EndCurrentAnimation(timeEnd);
        _timeOut = timeEnd;
    }

    public override void ExitState()
    {
        base.ExitState();
        ComboChecker();
        _fallingController.ResetFalling();

        if (!_nextAttackIsBit)
        {
            playerFSM.BitsController.SetBits(true);
        }
    }

    private void ComboChecker()
    {
        if (playerFSM.Combo >= _bitWeapons.Count - 1 && _nextAttackIsBit)
        {
            playerFSM.Combo = 0;
        }
        else if (_lastAction == EnumPlayerControlActions.None || !_fallingController.IsGrounded)
        {
            playerFSM.Combo = 0;
        }
        else
        {
            playerFSM.Combo++;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}