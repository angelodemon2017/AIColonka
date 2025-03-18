using System;

[Serializable]
[SignalClass("Set Task")]
public class SetTaskSignal
{
    public TaskSO Task;

    public SetTaskSignal(TaskSO task)
    {
        Task = task;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class SignalClassAttribute : Attribute
{
    public string DisplayName { get; }

    public SignalClassAttribute(string displayName = null)
    {
        DisplayName = displayName;
    }
}/**/