using UnityEngine;

public class AVWeapon : Weapon
{
    [SerializeField] protected AttackDecal _attackDecalPrefab;
    [SerializeField] protected float _sizeDecal;
    protected int _levelAVW;
    private bool _isAir;

    internal void InitAVW(int level, bool isAir = false)
    {
        _isAir = isAir;
        _levelAVW = level;
    }

    internal override void StartAttack()
    {
        transform.position = _avTransform.position;

        if (_target)
        {
            transform.LookAt(_target);
        }
        else
        {
            transform.rotation =
                CameraController.Instance.IsLookingDown && !_isAir ?
                _avTransform.rotation :
                _rotate;
        }

        Shoot();
    }

    protected virtual void SpawnDecal()
    {

    }

    protected virtual void Shoot() { }
}