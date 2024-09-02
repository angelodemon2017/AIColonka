using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [SerializeField] private List<Transform> _bits = new();
    [SerializeField] private GameObject _bit;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _speedRotate;

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

            for (int b = 0; b < _bits.Count; b++)
            {
                _bits[b].rotation = Quaternion.Euler(0f, 360f / _bits.Count * b, 0f);
            }
        }

        foreach (var b in _bits)
        {
            b.Rotate(0f, _speedRotate, 0f);
        }
    }
}