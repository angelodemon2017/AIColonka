using UnityEngine;

public class BitWeapon : Weapon
{
    [SerializeField] private float _timeForEndAttack;
    private PlayerBitAttackState _playerBitAttackState;
    private Points _points;

    protected Points Points => _points;

    /// <summary>
    /// Count of bit
    /// </summary>
    protected int BitLevel => ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits;

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