using System;
using System.Collections;
using UnityEngine;

public class PeriodicActivator : MonoBehaviour
{
    /// <summary>
    /// order arg
    /// </summary>
    private Action<int> _periodicAction;
    private float _periodTime;

    private Coroutine _cashCoroutine;

    internal void InitAndStart(Action<int> periodicAction, int count = 1, float periodTime = 0.1f, Action endActions = null)
    {
        _periodicAction = periodicAction;
        _periodTime = periodTime;

        _cashCoroutine = StartCoroutine(CallFunc(count, endActions));
    }

    internal void StopActions()
    {
        if (_cashCoroutine != null)
        {
            StopCoroutine(_cashCoroutine);
            _cashCoroutine = null;
        }
    }

    private IEnumerator CallFunc(int count, Action endAction = null)
    {
        _periodicAction?.Invoke(count);

        yield return new WaitForSeconds(_periodTime);

        if (count > 1)
        {
            _cashCoroutine = StartCoroutine(CallFunc(count - 1, endAction));
        }
        else
        {
            endAction?.Invoke();
        }
    }
}