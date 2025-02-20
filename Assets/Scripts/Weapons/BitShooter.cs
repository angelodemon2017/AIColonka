using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitShooter : Weapon
{
    [SerializeField] private List<DamageConfig> _damageConfigs = new();
    [SerializeField] private Projectile _projectile;

    private DamageConfig _curDamConf;
    private PlayerBitAttackState _playerBitAttackState;

    internal override void StartAttack()
    {
        base.StartAttack();

        _curDamConf = _damageConfigs[Mathf.Clamp(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits, 0, _damageConfigs.Count - 1)];

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

        bit.SetDamage(new Damage(EnumDamageType.BitRange, 
            _curDamConf.BaseDamage + (int)(_curDamConf.MultOrder * ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits - count)));

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

    [System.Serializable]
    public class DamageConfig
    {
        public float WaitBetweenShoot;
        public int BaseDamage;
        public float MultOrder;
    }
}