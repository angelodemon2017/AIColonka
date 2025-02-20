using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitShooter : BitWeapon
{
    [SerializeField] private List<DamageConfig> _damageConfigs = new();
    [SerializeField] private Projectile _projectile;

    private DamageConfig _curDamConf;

    internal override void StartAttack()
    {
        base.StartAttack();

        transform.position = Points.PointOfLookCamera.position;
        transform.rotation = Camera.main.transform.rotation;

        _curDamConf = _damageConfigs[Mathf.Clamp(BitLevel, 0, _damageConfigs.Count - 1)];

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
            _curDamConf.BaseDamage + (int)(_curDamConf.MultOrder * BitLevel - count)));

        yield return new WaitForSeconds(0.15f);

        if (count > 0)
        {
            StartCoroutine(Shoot(count - 1));
        }
        else
        {
            EndBitAttack();
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