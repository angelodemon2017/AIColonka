using System.Collections.Generic;
using UnityEngine;

public class MAINWindow : MonoBehaviour, IWindowFSM
{
    [SerializeField] private List<PareEventWindow> _pareEventWindows = new();
//    private Dictionary<EnumUIEvent, MAINWindow> _tempMapWindows = new();

    private Dictionary<KeyCode, MAINWindow> _tempKeyCodeMapWindows = new();

    private void InitWindows()
    {
        _pareEventWindows.ForEach(p => _tempKeyCodeMapWindows.Add(p.keyCode, p.Window));
            //_tempMapWindows.Add(p.uIEvent, p.Window));
    }

    public virtual void Run()
    {
        foreach (var kc in _tempKeyCodeMapWindows)
        {
            if (Input.GetKeyDown(kc.Key))
            {
                UIFSM.Instance.OpenWindow(kc.Value);
            }
        }
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