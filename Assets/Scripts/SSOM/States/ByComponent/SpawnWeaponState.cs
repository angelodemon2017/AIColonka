using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/SpawnWeaponState", order = 1)]
public class SpawnWeaponState : State, IWVState
{
    [SerializeField] protected List<int> _countLaunched;
    [SerializeField] protected List<float> _intervalsLaunched;
    [SerializeField] protected List<Weapon> _weapons;
    [SerializeField] private string _customKey;

    protected EntityModule _entityModule;

    private int GetPhase => (_entityModule && _entityModule is IPhaselable phas) ? phas.GetPhase() : 0;

    public virtual int GetCountLaunched =>
        _countLaunched.Count > 0 ?
            _countLaunched.GetBorderElement(GetPhase) : 1;
    public virtual float GetIntervalLaunched =>
        _intervalsLaunched.Count > 0 ?
            _intervalsLaunched.GetBorderElement(GetPhase) : 0.1f;
    internal virtual int GetLevel => 1;//need?
    public virtual Weapon GetWeapon => _weapons.GetBorderElement(GetPhase);
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
            emwws.GetWeaponVisualizator.CallAttack(this);
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