using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/SpawnWeaponState", order = 1)]
public class SpawnWeaponState : State
{
    private WeaponVisualizator _weaponVisualizator;
    [SerializeField] private int _levelWeapon;
    [SerializeField] private Weapon _dummyWeapon;

    internal virtual int GetCountLaunched => 1;
    internal virtual float GetIntervalLaunched => 0.1f;
    internal virtual int GetLevel => _levelWeapon;
    internal virtual Weapon GetWeapon => _dummyWeapon;

    protected override void Init()
    {
        base.Init();

        //TODO getting _weaponVisualizator

        _weaponVisualizator.CallAttack(this);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}