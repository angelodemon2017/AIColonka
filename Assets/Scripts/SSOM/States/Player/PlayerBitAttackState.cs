using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerBitAttackState", order = 1)]
public class PlayerBitAttackState : PlayerState
{
    [SerializeField] private List<BitWeapon> _bitWeapons = new();

    private FallingController _fallingController;

    private EnumPlayerControlActions _lastAction = EnumPlayerControlActions.None;

    private float _timeOut = 0f;

    protected override void Init()
    {
        base.Init();
        playerFSM.BitsController.SetBits(false);

        _fallingController = playerFSM.GetFallingController;
        _fallingController.SwitchGravity();

        var w = Instantiate(_bitWeapons.GetRandom());

        w.SetPBAS(this, playerFSM.GetPoints);
        w.Init(EnumWhoIs.Player,
            playerFSM.GetPoints.PointOfLookCamera,
            playerFSM.GetPoints.EnemyIsTarget ?
                playerFSM.GetPoints.TargetEnemy.transform :
                null,
            Camera.main.transform.rotation);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        playerFSM.AnimationAdapter.transform.rotation = Quaternion.LookRotation(forward);
    }

    internal override void CallPlayerAction(EnumPlayerControlActions playerAction)
    {
        _lastAction = playerAction;
    }

    protected override void Run()
    {
        base.Run();

        if (_timeOut > 0f)
        {
            _timeOut -= Time.deltaTime;
            if (_timeOut <= 0f)
            {
                IsFinished = true;
            }
        }
    }

    internal override void EndCurrentAnimation(float timeEnd)
    {
        base.EndCurrentAnimation(timeEnd);
        _timeOut = timeEnd;
    }

    public override void ExitState()
    {
        base.ExitState();
        _fallingController.ResetFalling();

        if (_lastAction == EnumPlayerControlActions.None)
        {
            playerFSM.Combo = 0;
        }
        else
        {
            playerFSM.Combo++;
        }

        playerFSM.BitsController.SetBits(true);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}