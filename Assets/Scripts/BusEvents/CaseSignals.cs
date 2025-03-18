using System;
using Zenject;

[Serializable]
public class CaseSignals
{
    public TypeSignal typeSignal;
    [SignalType(TypeSignal.SetTask)]
    public SetTaskSignal _setTaskSignal;
    [SignalType(TypeSignal.BackTalk)]
    public BackTalkSignal _backTalkSignal;
    [SignalType(TypeSignal.SetWindow)]
    public SetWindowSignal _setWindowSignal;
    [SignalType(TypeSignal.SetNextDialog)]
    public SetNextDialogSignal _setNextDialogSignal;
    
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
            case TypeSignal.SetWindow:
                signalBus.Fire(_setWindowSignal);
                break;
            case TypeSignal.SetNextDialog:
                signalBus.Fire(_setNextDialogSignal);
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
        SetWindow,
        SetNextDialog,
    }
}