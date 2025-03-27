using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/Levels/FlyByRocketState", order = 1)]
public class FlyByRocketState : PlayerState
{
    private PositionLerper _looker;
    private PositionLerper _rocketLerper;
    private PositionLerper _cameraLerper;

    protected override void Init()
    {
        base.Init();
    }

    internal void CustomInit(PositionLerper looker, PositionLerper rocketLerper, PositionLerper cameraLerper)
    {
        _looker = looker;
        _rocketLerper = rocketLerper;
        _cameraLerper = cameraLerper;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        _rocketLerper.Move(hor);
        _looker.Move(hor);
        _cameraLerper.Move(hor);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}