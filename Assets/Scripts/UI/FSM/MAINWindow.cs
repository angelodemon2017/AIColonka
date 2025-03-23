using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MAINWindow : MonoBehaviour, IWindowFSM
{
    [SerializeField] private bool _needTransition;
    [SerializeField] private List<PareEventWindow> _pareEventWindows = new();

    private Dictionary<KeyCode, MAINWindow> _tempKeyCodeMapWindows = new();

    [Inject]
    private SignalBus _signalBus;

    public bool NeedTransition => _needTransition;

    private void InitWindows()
    {
        _pareEventWindows.ForEach(p => _tempKeyCodeMapWindows.Add(p.keyCode, p.Window));
    }

    public virtual void Run()
    {
        foreach (var kc in _tempKeyCodeMapWindows)
        {
            if (Input.GetKeyDown(kc.Key))
            {
                _signalBus.Fire(new SetWindowSignal(kc.Value));
            }
        }
    }

    public virtual void FixedRun()
    {

    }

    public virtual void StartWindow()
    {
        InitWindows();
    }

    public virtual void ExitWindow()
    {

    }

    [System.Serializable]
    internal class PareEventWindow
    {
        public KeyCode keyCode;
        public MAINWindow Window;
    }
}