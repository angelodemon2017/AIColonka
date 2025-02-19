using System.Collections;
using UnityEngine;

public class UIFSM : MonoBehaviour, IUIFSM
{
    public static UIFSM Instance;

    [SerializeField] private MAINWindow _startWindow;

    private Transform _parent;

    private IWindowFSM _currentWindow;

    private void Awake()
    {
        _parent = transform;
        Instance = this;
        OpenWindow(_startWindow);
    }

    private void Update()
    {
        _currentWindow.Run();
    }

    private void FixedUpdate()
    {
        _currentWindow.FixedRun();
    }

    public MAINWindow OpenWindow(MAINWindow windowFSM)
    {
        if (_currentWindow != null)
        {
            _currentWindow.ExitWindow();
        }
        //place for pool
        _parent.DestroyChildrens();

        _currentWindow = Instantiate(windowFSM, _parent);
        StartWindow();

        return _currentWindow as MAINWindow;
    }

    public void StartWindow()
    {
        if (_currentWindow != null)
        {
            _currentWindow.StartWindow(); 
        }
    }

    private void OnDestroy()
    {
        if (_currentWindow != null)
        {
            _currentWindow.ExitWindow();
        }
    }
}