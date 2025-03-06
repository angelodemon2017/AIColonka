using UnityEngine;

public class WVEntityModule : EntityModule, IEntityModuleWithWeaponSpawner
{
    [SerializeField] private WeaponVisualizator _weaponVisualizator;

    public WeaponVisualizator GetWeaponVisualizator => _weaponVisualizator;

    internal override void Init()
    {
        base.Init();
    }
}