using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeathWindow : MAINWindow
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private UnityEvent _unityEvent;
    
    public override void StartWindow()
    {
        base.StartWindow();
        _restartButton.onClick.AddListener(RestartClick);
    }

    private void RestartClick()
    {
        _unityEvent?.Invoke();
    }
}