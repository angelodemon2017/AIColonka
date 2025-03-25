using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class HPComponent : MonoBehaviour
{
    [SerializeField] private float TimeoutRegen;
    [SerializeField] private UnityEvent _eventByDeath;
    [SerializeField] private int MaxHP;
    [SerializeField] private int CurrentHP;
    [SerializeField] private int _regenHP;
    [SerializeField] private float _immuAfterDamage;
    [SerializeField] private SignalAgregator _deathSignal;

    private SignalBus _signalBus;
    private float _immuneTime;
    private float _lastHP;
    private float _timeOut;
    private bool _isPlayer;
    private bool _isDeath = false;

    internal Action Death;
    internal Action<float, float, float> ChangeHP;

    /// <summary>
    /// 0-1
    /// </summary>
    internal float GetPercentHP => (float)CurrentHP / MaxHP;
    internal bool IsAlive => CurrentHP > 0;

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        CurrentHP = MaxHP;
    }

    internal void OverrideStats(int maxHp, int regenHP)
    {
        _isPlayer = true;
        MaxHP = maxHp;
        CurrentHP = MaxHP;
        _regenHP = regenHP;

        OnChangeHP();
    }

    internal void SetImmune(float immuTime)
    {
        _immuneTime = immuTime;
    }

    internal void GetDamage(int damageCount)
    {
        if (_immuneTime > 0)
        {
            return;
        }
        if(_isPlayer)
            _signalBus.Fire(new PlayerDamageSignal());

        _lastHP = CurrentHP;
        _immuneTime = _immuAfterDamage;
        CurrentHP -= damageCount;
        _timeOut = TimeoutRegen;
        if (CurrentHP <= 0)
        {
            Kill();
        }
        OnChangeHP();
    }

    internal void Heal(int healAmount)
    {
        if (_isPlayer)
            _signalBus.Fire(new PlayerHealSignal());
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
        if (_isDeath)
        {
            return;
        }
        _isDeath = true;

        CurrentHP = 0;
        _deathSignal.FireAll(_signalBus);
        Death?.Invoke();
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
        if (_immuneTime > 0)
        {
            _immuneTime -= Time.deltaTime;
        }
    }
}