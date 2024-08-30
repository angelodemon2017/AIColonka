using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    public bool IsResetState { get; protected set; }
    [HideInInspector] public IStatesCharacter Character;
    [SerializeField] protected float speed = 0.05f;
    [SerializeField] protected float check_distance = 0.5f;
    [SerializeField] protected List<State> AvailableStates;

    public virtual void Init(IStatesCharacter character) { }

    public abstract void Run();

    public abstract bool CheckRules(IStatesCharacter character);
}