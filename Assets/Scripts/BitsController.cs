using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private List<Transform> _bits = new();
    [SerializeField] private GameObject _bit;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _speedRotate;
    [SerializeField] private BitScript _bitScript;
    [SerializeField] private float _speedShoot;

    private int _valueDamage = 1;
    private Damage damage => new Damage(EnumDamageType.BitRange, _valueDamage, true);

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var nb = Instantiate(_bit);
            nb.transform.SetParent(_parent);
            nb.transform.position = _parent.position;
            _bits.Add(nb.transform);

            CorrectOrbit();
        }

        foreach (var b in _bits)
        {
            b.Rotate(0f, _speedRotate, 0f);
        }
    }

    public void Shoot()
    {
        if (_bits.Count == 0)
            return;

        var b = _bits[0];
        _bits.Remove(b);
        Destroy(b.gameObject);
        CorrectOrbit();

        var bullet = Instantiate(_bitScript, _attackPoint.position, transform.rotation);
        var dir = CameraController.Instance.Direct;
        bullet.Init(dir * _speedShoot, damage);
    }

    private void CorrectOrbit()
    {
        for (int b = 0; b < _bits.Count; b++)
        {
            _bits[b].rotation = Quaternion.Euler(0f, 360f / _bits.Count * b, 0f);
        }
    }
}