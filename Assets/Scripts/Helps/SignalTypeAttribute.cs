using System;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class SignalTypeAttribute : Attribute
{
    public CaseSignals.TypeSignal TypeSignal { get; }

    public SignalTypeAttribute(CaseSignals.TypeSignal typeSignal)
    {
        TypeSignal = typeSignal;
    }
}