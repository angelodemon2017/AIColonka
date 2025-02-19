using System.Collections.Generic;
using UnityEngine;

public class BitOrbit : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;

    [SerializeField] private List<Transform> _bits = new();
    private List<MeshRenderer> _mrs = new();

    internal Rotator Rotator => _rotator;

    private void Awake()
    {
        _bits.ForEach(b => _mrs.Add(b.GetComponentInChildren<MeshRenderer>()));
    }

    internal void SetMaterial(Material material)
    {
        _mrs.ForEach(m => m.material = material);
    }

    internal void SetBits(int count)
    {
        for (int i = 0; i < _bits.Count; i++)
        {
            _bits[i].gameObject.SetActive(i < count);
            if (i < count)
            {
                _bits[i].localRotation = Quaternion.Euler(0f, 360f / count * i, 0f);
            }
        }
    }
}