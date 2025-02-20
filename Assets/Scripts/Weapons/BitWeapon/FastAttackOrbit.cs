using System.Collections;
using UnityEngine;

public class FastAttackOrbit : BitWeapon
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private BitOrbit _bitOrbit;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private float _delayBetweenBits;
    [SerializeField] private int _stepAccelerate = 5;
    [SerializeField] private float _precentAccelerate = 50f;
    [SerializeField] private float _delayStepAccelrate = 0.2f;
    [SerializeField] private float _timeLifeAfterEnd = 1f;

    private Vector3 _swiftPos = Vector3.zero;

    protected virtual float Yswift => Random.Range(0.25f, 0.75f);
    protected virtual Transform spawnPoint => Points.PointOfTargetForEnemy;

    internal override void StartAttack()
    {
        base.StartAttack();

        transform.position = spawnPoint.position;

        StartCoroutine(AddBit(BitLevel - 1));
    }

    IEnumerator AddBit(int countOrder)
    {
        _swiftPos.y = Yswift;
        var newW = Instantiate(_weaponPrefab, transform.position + _swiftPos, transform.rotation, transform);
        newW.SetDamage(_damage);
        _bitOrbit.AddBitTransform(newW.transform);
        newW.Init(WhoIs.whoIs);
        _bitOrbit.ShowAll();

        yield return new WaitForSeconds(_delayBetweenBits);

        if (countOrder > 0)
        {
            StartCoroutine(AddBit(countOrder - 1));
        }
        else
        {
            StartCoroutine(AddSpeed(_stepAccelerate));
            EndBitAttack();
        }
    }

    IEnumerator AddSpeed(int iter)
    {
        _rotator.AddSpeed(_precentAccelerate);

        yield return new WaitForSeconds(_delayStepAccelrate);

        if (iter > 0)
        {
            StartCoroutine(AddSpeed(iter - 1));
        }
        else
        {
            Destroy(gameObject, _timeLifeAfterEnd);
        }
    }
}