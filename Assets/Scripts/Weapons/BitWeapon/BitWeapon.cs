using UnityEngine;
using Zenject;

public class BitWeapon : Weapon
{
    [SerializeField] private float _timeForEndAttack;

    private PlayerBitAttackState _playerBitAttackState;
    private Points _points;
    private DataHandler _dataHandler;

    protected Points Points => _points;
    protected PlayerBitAttackState PlayerBitAttackState => _playerBitAttackState;

    /// <summary>
    /// Count of bit
    /// </summary>
    protected int BitLevel => _dataHandler.CurrentData.gamePlayProgress.BattleBits;

    [Inject]
    private void Construct(
        DataHandler dataHandler)
    {
        _dataHandler = dataHandler;
    }

    internal void SetPBAS(PlayerBitAttackState playerBitAttackState, Points points)
    {
        _playerBitAttackState = playerBitAttackState;
        _points = points;
    }

    protected void EndBitAttack()
    {
        _playerBitAttackState?.EndCurrentAnimation(_timeForEndAttack);
    }
}