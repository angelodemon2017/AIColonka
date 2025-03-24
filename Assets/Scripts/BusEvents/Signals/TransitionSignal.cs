using System;

[Serializable]
public class TransitionSignal
{
    public bool IsInverse;
    public Action ActionByEnd;

    public TransitionSignal(bool isInverse = false, Action actionByEnd = null)
    {
        IsInverse = isInverse;
        ActionByEnd = actionByEnd;
    }
}