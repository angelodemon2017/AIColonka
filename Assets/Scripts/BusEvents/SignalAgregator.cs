using System;
using System.Collections.Generic;
using Zenject;

[Serializable]
public class SignalAgregator
{
    public List<CaseSignals> _caseSignals = new List<CaseSignals>();

    internal void FireAll(SignalBus signalBus)
    {
        foreach (var s in _caseSignals)
        {
            s.Fire(signalBus);
        }
    }
}