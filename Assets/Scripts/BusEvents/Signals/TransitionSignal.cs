using System;

[Serializable]
public class TransitionSignal
{
    public bool IsInverse;

    public TransitionSignal(bool isInverse = false)
    {
        IsInverse = isInverse;
    }
}