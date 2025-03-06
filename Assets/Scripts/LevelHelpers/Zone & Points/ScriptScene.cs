using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptScene : MonoBehaviour
{
    [SerializeField] private List<UnityEvent> _events;
    [SerializeField] private UnityEvent _eventOnEnd;

    private int _scriptStep = 0;

    public void RunScene()
    {
        _scriptStep = 0;
        RunStep();
    }

    public void RunNextStep()
    {
        _scriptStep++;
        RunStep();
    }

    public void RunStep()
    {
        if (_scriptStep < _events.Count)
        {
            _events[_scriptStep].Invoke();
        }
        else
        {
            Debug.LogWarning($"Script Step {_scriptStep} havn't in _events");
        }
    }

    internal void EndScript()
    {
        _eventOnEnd?.Invoke();
    }
}