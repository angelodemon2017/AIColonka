using UnityEngine;
using Zenject;

public interface IStatesCharacter
{
    EntityModule GetModule { get; }
    DiContainer Container { get; }

    bool IsFinishedCurrentState();
    Transform GetTransform();
    void PlayAnimation(EnumAnimations animation);
    void SetState(State state, bool ignoreEqual = false);
}