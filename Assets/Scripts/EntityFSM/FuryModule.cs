using UnityEngine;
using UnityEngine.Events;

public class FuryModule : EntityModule, IEntityModuleWithWeaponSpawner, IPhaselable
{
    [SerializeField] private WeaponVisualizator _weaponVisualizator;
    [SerializeField] private AnimationCurve _phaseByHP;
    [SerializeField] private UnityEvent _eventByChangeHP;

    private int _lastPhase;

    public WeaponVisualizator GetWeaponVisualizator => _weaponVisualizator;

    internal override void Init()
    {
        base.Init();

        _lastPhase = GetPhase();
        _hPComponent.ChangeHP += UpdateHP;
    }

    private void UpdateHP(float a, float b, float c)
    {
        if (_lastPhase != GetPhase())
        {
            _eventByChangeHP?.Invoke();
            _lastPhase = GetPhase();
        }
    }

    public int GetPhase()
    {
        return (int)_phaseByHP.Evaluate(_hPComponent.GetPercentHP);
    }
}