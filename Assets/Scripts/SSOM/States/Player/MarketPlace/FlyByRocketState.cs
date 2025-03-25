using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/Levels/FlyByRocketState", order = 1)]
public class FlyByRocketState : PlayerState
{
    private PositionLerper _moving;
    private PositionLerper _rocketLerper;

    protected override void Init()
    {
        base.Init();
    }

    internal void CustomInit(PositionLerper moving, PositionLerper rocketLerper)
    {
        _moving = moving;
        _rocketLerper = rocketLerper;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
//        _moving.Move(hor);
        _rocketLerper.Move(hor);
//        _looker.Move(hor);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}