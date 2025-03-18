using System.Collections.Generic;
using Zenject;

[System.Serializable]
public class SignalAgregator
{
    public List<CustomSignal> _customSignals;
    public List<SetTaskSignal> _setTasks;
    public List<BackTalkSignal> _backTalks;

    internal void FireAll(SignalBus signalBus)
    {
        foreach (var t in _setTasks)
        {
            signalBus.Fire(t);
        }
    }
}

[System.Serializable]
public class CustomSignal
{
    public TypeSignal typeSignal;
    public SetTaskSignal _setTaskSignal;
    public BackTalkSignal _backTalkSignal;

    public void Fire(SignalBus signalBus)
    {
        switch (typeSignal)
        {
            case TypeSignal.SetTask:
                signalBus.Fire(_setTaskSignal);
                break;
            case TypeSignal.BackTalk:
                signalBus.Fire(_backTalkSignal);
                break;
            default:
                break;
        }
    }

    public enum TypeSignal
    {
        None,
        SetTask,
        BackTalk,
    }
}