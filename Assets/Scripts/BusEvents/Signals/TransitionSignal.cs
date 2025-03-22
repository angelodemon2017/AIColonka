using System;

[Serializable]
public class TransitionSignal
{
    public Action SomeAction;

    public TransitionSignal(Action someAction)
    {
        SomeAction = someAction;
    }
}