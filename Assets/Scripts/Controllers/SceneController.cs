using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Image _blackImage;
    [SerializeField] private TextMeshProUGUI _testLoading;

    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<SetLevelSignal>(LoadLevelBySignal);
    }

    private void LoadLevelBySignal(SetLevelSignal setLevelSignal)
    {

    }


}