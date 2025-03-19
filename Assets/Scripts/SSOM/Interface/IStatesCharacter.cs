using UnityEngine;
using Zenject;

public interface IStatesCharacter
{
    EntityModule GetModule { get; }
    bool IsFinishedCurrentState();

    Transform GetTransform();

    DiContainer Container { get; }

    void PlayAnimation(EnumAnimations animation);

    void SetState(State state, bool ignoreEqual = false);
}