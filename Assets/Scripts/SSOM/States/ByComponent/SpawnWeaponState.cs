using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/SpawnWeaponState", order = 1)]
public class SpawnWeaponState : State
{
    [SerializeField] protected List<int> _countLaunched;
    [SerializeField] protected List<float> _intervalsLaunched;
    [SerializeField] protected List<Weapon> _weapons;
    [SerializeField] private string _customKey;

    private WeaponVisualizator _weaponVisualizator;
    protected EntityModule _entityModule;

    private int GetPhase => _entityModule is IPhaselable phas ? phas.GetPhase() : 0;

    internal virtual int GetCountLaunched =>
        _countLaunched.Count > 0 ?
            _countLaunched.GetBorderElement(GetPhase) : 1;
    internal virtual float GetIntervalLaunched =>
        _intervalsLaunched.Count > 0 ?
            _intervalsLaunched.GetBorderElement(GetPhase) : 0.1f;
    internal virtual int GetLevel => 1;//need?
    internal virtual Weapon GetWeapon => _weapons.GetBorderElement(GetPhase);
    internal override string Key => _customKey;

    private void OnValidate()
    {
        if (_weapons.Count == 0)
        {
            Debug.LogError("Need weapon variants");
        }
    }

    protected override void Init()
    {
        base.Init();

        _entityModule = Character.GetModule;

        if (_entityModule is IEntityModuleWithWeaponSpawner emwws)
        {
            _weaponVisualizator = emwws.GetWeaponVisualizator;
            _weaponVisualizator.CallAttack(this);
        }
        else
        {
            Finish();
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}