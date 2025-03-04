using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/SWSetShieldState", order = 1)]
public class SWSetShieldState : SpawnWeaponState
{
    [SerializeField] private float _immuTime;

    protected override void Init()
    {
        base.Init();

        if (!IsFinished)
        {
//            Character.GetModule.hPComponent.SetImmune(_immuTime);
        }
    }
}