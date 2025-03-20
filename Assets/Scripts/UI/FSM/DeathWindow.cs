using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeathWindow : MAINWindow
{
    [SerializeField] private Button _restartButton;

    [Inject]
    private SceneController _sceneController;

    public override void StartWindow()
    {
        base.StartWindow();
        _restartButton.onClick.AddListener(RestartClick);
    }

    private void RestartClick()
    {
        _sceneController.Restart();
    }
}