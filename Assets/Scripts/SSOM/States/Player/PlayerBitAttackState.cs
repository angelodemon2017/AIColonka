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
    internal bool IsAir => !_playerFSM.GetFallingController.IsGrounded;

    protected override void Init()
    {
        base.Init();

        _fallingController = _playerFSM.GetFallingController;

        if (IsAir && !_fallingController.AvailableActionInAir)
        {
            IsFinished = true;
            return;
        }

        _fallingController.SwitchGravity();

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        _playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(forward);

        _playerFSM.BitsController.SetBits(false);

        InitBitWeapon();
    }

    private void InitBitWeapon()
    {
        var w = _diContainer.InstantiatePrefabForComponent<BitWeapon>(_bitWeapons.GetBorderElement(_gameplayHandler.Combo));

        w.SetPBAS(this, _playerFSM.GetPoints);
        w.Init(EnumWhoIs.Player,
            _playerFSM.GetPoints.PointOfLookCamera,
            _playerFSM.GetPoints.EnemyIsTarget ?
                _playerFSM.GetPoints.TargetEnemy.transform :
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
            _playerFSM.BitsController.SetBits(true);
        }
    }

    private void ComboChecker()
    {
        if (_gameplayHandler.Combo >= _bitWeapons.Count - 1 && _nextAttackIsBit)
        {
            _gameplayHandler.Combo = 0;
        }
        else if (_lastAction == EnumPlayerControlActions.None || !_fallingController.IsGrounded)
        {
            _gameplayHandler.Combo = 0;
        }
        else
        {
            _gameplayHandler.Combo++;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}