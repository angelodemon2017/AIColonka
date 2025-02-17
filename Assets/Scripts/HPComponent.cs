using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HPComponent : MonoBehaviour
{
    [SerializeField] private float TimeoutRegen;
    [SerializeField] private UnityEvent _eventByDeath;
    [SerializeField] private int MaxHP;
    [SerializeField] private int CurrentHP;
    [SerializeField] private int _regenHP;

    private float _lastHP;
    private float _timeOut;

    internal Action<float, float, float> ChangeHP;

    private void Awake()
    {
        CurrentHP = MaxHP;
    }

//    IEnumerator CrunchUpdate

    internal void OverrideStats(int maxHp, int regenHP)
    {
        MaxHP = maxHp;
        CurrentHP = MaxHP;
        _regenHP = regenHP;

        OnChangeHP();
    }

    internal void GetDamage(int damageCount)
    {
        _lastHP = CurrentHP;
        Debug.Log($"Getting damage {damageCount}");
        CurrentHP -= damageCount;
        _timeOut = TimeoutRegen;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Kill();
        }
        OnChangeHP();
    }

    internal void Heal(int healAmount)
    {
        CurrentHP += healAmount;
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
        OnChangeHP();
    }

    internal void OnChangeHP()
    {
        ChangeHP?.Invoke(_lastHP, CurrentHP, MaxHP);
    }

    public void Kill()
    {
        CurrentHP = 0;
        _eventByDeath?.Invoke();
    }

    private void Update()
    {
        if (CurrentHP > 0 && _regenHP > 0)
        {
            if (_timeOut > 0f)
            {
                _timeOut -= Time.deltaTime;
            }
            else if (CurrentHP < MaxHP)
            {
                Heal(_regenHP);
                _timeOut = 0.5f;
            }
        }
    }
}