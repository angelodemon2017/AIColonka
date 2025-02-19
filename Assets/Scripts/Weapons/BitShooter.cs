using System.Collections;
using UnityEngine;

public class BitShooter : Weapon
{
    [SerializeField] private Projectile _projectile;

    private PlayerBitAttackState _playerBitAttackState;

    internal override void StartAttack()
    {
        base.StartAttack();

        if (PlayerFSM.Instance.GetPoints.EnemyIsTarget)
        {
            transform.LookAt(PlayerFSM.Instance.GetPoints.TargetEnemy.transform);
        }
        else
        {
            transform.rotation = _rotate;
        }

        StartCoroutine(Shoot(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits - 1));
    }

    internal void SetPBAS(PlayerBitAttackState playerBitAttackState)
    {
        _playerBitAttackState = playerBitAttackState;
    }

    IEnumerator Shoot(int count)
    {
        var fromVect = new Vector3(Random.Range(-0.3f, 0.3f),
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.3f, 0.3f));

        var bit = Instantiate(_projectile, transform.position + fromVect, transform.rotation);
        bit.Init(
            WhoIs.whoIs,
//            transform,
            target: _target,
            rotation: transform.rotation);

        yield return new WaitForSeconds(0.15f);

        if (count > 0)
        {
            StartCoroutine(Shoot(count - 1));
        }
        else
        {
            _playerBitAttackState?.EndCurrentAnimation();
        }
    }
}