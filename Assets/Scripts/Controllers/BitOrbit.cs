using System.Collections;
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
        SetBits(0);
    }

    internal void SetMaterial(Material material)
    {
        _mrs.ForEach(m => m.material = material);
    }

    internal void SetBits(int count, int addOrder = 0, bool isOn = true)
    {
        for (int i = 0; i < _bits.Count; i++)
        {
//            _bits[i].gameObject.SetActive(i < count);
            StartCoroutine(OnOffBit(i < count && isOn, i, i + addOrder));
            if (i < count)
            {
                _bits[i].localRotation = Quaternion.Euler(0f, 360f / count * i, 0f);
            }
        }
    }

    IEnumerator OnOffBit(bool isOn, int index, int order)
    {
        yield return new WaitForSeconds(0.1f * order);

        _bits[index].gameObject.SetActive(isOn);
    }

/*    internal void OnOffBits(int count, float waiter, bool isOn)
    {
        _currentCount = count;
        StartCoroutine(Shower(count, waiter, isOn));
    }

    private int _currentCount;
    IEnumerator Shower(int count, float waiter, bool isOn)
    {
        yield return new WaitForSeconds(waiter);

        _bits[_currentCount - count].gameObject.SetActive(isOn);
        _bits[_currentCount - count].localRotation = 
            Quaternion.Euler(0f, 360f / ((_currentCount - count > 0) ? (_currentCount * (_currentCount - count)) : 1), 0f);
        if (count > 0)
        {
            StartCoroutine(Shower(count - 1,
                waiter > 0.1 ? 0.1f : waiter, isOn));
        }
    }/**/
}