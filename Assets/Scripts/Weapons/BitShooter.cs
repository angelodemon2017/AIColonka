using System.Collections.Generic;
using UnityEngine;

public class BitShooter : BitWeapon
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private List<DamageConfig> _damageConfigs = new();
    [SerializeField] private Projectile _projectile;

    private DamageConfig _curDamConf;

    internal override void StartAttack()
    {
        base.StartAttack();

        if (PlayerBitAttackState.IsAir)
        {
            if (Points.EnemyIsTarget)
            {
                var tempPos = _target.position;
                tempPos.y = Points.PointOfTargetForEnemy.position.y + 5f;
                transform.position = tempPos;

                transform.LookAt(Points.TargetEnemy.transform);
            }
            else
            {
                Vector3 forward = Camera.main.transform.forward.normalized * 2f;
                forward.y = 2f;

                var tempPos = Points.PointOfTargetForEnemy.position;
                transform.position = tempPos + forward;

                transform.rotation = Quaternion.LookRotation(Vector3.down);
            }
        }
        else
        {
            var tempPos = Points.PointOfLookCamera.position;
            tempPos.y += 0.5f;
            transform.position = tempPos;

            if (Points.EnemyIsTarget)
            {
                transform.LookAt(_target);
            }
            else
            {
                transform.rotation = _rotate;
            }
        }

        _curDamConf = _damageConfigs[Mathf.Clamp(BitLevel, 0, _damageConfigs.Count - 1)];

        _periodicActivator.InitAndStart(SpawnAndShoot, BitLevel - 1, 0.15f, EndSpawning);
    }

    private void SpawnAndShoot(int count)
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
            _curDamConf.BaseDamage + (int)(_curDamConf.MultOrder * BitLevel - count)));
    }

    private void EndSpawning()
    {
        EndBitAttack();
        Destroy(gameObject);
    }

    [System.Serializable]
    public class DamageConfig
    {
        public float WaitBetweenShoot;
        public int BaseDamage;
        public float MultOrder;
    }
}