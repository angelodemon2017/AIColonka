using UnityEngine;

public interface IStatesCharacter
{
    EntityModule GetModule { get; }
    bool IsFinishedCurrentState();

    Transform GetTransform();

    void PlayAnimation(EnumAnimations animation);

    void SetState(State state, bool ignoreEqual = false);

    bool CheckProp(EnumProps prop);

    void TakeObject(GameObject keepObj);
    void InitAttackZone(GameObject attackZone);
}