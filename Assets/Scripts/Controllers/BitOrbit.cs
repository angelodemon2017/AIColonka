using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitOrbit : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;
    [SerializeField] private float _onOffPeriod = 0.1f;
    [SerializeField] private List<Transform> _bits = new();
    private List<MeshRenderer> _mrs = new();

    internal Rotator Rotator => _rotator;

    private void Awake()
    {
        _bits.ForEach(b => _mrs.Add(b.GetComponentInChildren<MeshRenderer>()));
        SetBits(0);
    }

    internal void SetMaterial(Material material)
    {
        _mrs.ForEach(m => m.material = material);
    }

    internal void AddBitTransform(Transform addTransform)
    {
        addTransform.SetParent(transform);
        _bits.Add(addTransform);
    }

    internal void ShowAll()
    {
        SetBits(_bits.Count);
    }

    internal void SetBits(int count, int addOrder = 0, bool isOn = true)
    {
        for (int i = 0; i < _bits.Count; i++)
        {
            StartCoroutine(OnOffBit(i < count && isOn, i, i + addOrder));
            if (i < count)
            {
                if (_bits[i])
                {
                    _bits[i].localRotation = Quaternion.Euler(0f, 360f / count * i, 0f);
                }
            }
        }
    }

    IEnumerator OnOffBit(bool isOn, int index, int order)
    {
        yield return new WaitForSeconds(_onOffPeriod * order);

        if (_bits[index])
        {
            _bits[index].gameObject.SetActive(isOn);
        }
    }
}