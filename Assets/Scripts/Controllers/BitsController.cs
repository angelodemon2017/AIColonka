using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BitsController : MonoBehaviour
{
    [SerializeField] private List<BitOrbit> _orbits = new();
    [SerializeField] private List<BitOrbitConfig> _bitOrbitConfigs = new();

    [SerializeField] private Material _peaceBitMaterial;
    [SerializeField] private Material _fightBitMaterial;

    private SignalBus _signalBus;
    private DataHandler _dataHandler;
    private GameplayHandler _gameplayHandler;

    private BitOrbitConfig currentConfig => _bitOrbitConfigs[currentBit];
    private int currentBit => _dataHandler.CurrentData.gamePlayProgress.BattleBits;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        DataHandler dataHandler,
        GameplayHandler gameplayHandler)
    {
        _signalBus = signalBus;
        _dataHandler = dataHandler;
        _gameplayHandler = gameplayHandler;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<BitUpgradedSignal>(ShowAll);
        _signalBus.Subscribe<GameModeSignal>(UpdateMode);

        _orbits.ForEach(o => o.Init());

        ShowAll();
    }

    private void ShowAll()
    {
        SetBits(true);
    }

    internal void UpdateMode()
    {
        var isf = _gameplayHandler.FightMode;
        foreach (var o in _orbits)
        {
            o.Rotator.SetSpeed(isf ? 5f : 2f);
            o.SetMaterial(isf ? _fightBitMaterial : _peaceBitMaterial);
        }
    }

    internal void SetBits(bool isOn)
    {
        var config = currentConfig;
        for (int i = 0; i < 3; i++)
        {
            _orbits[i].SetBits(config.orbitConfigs[i].countBit, CountBefore(i), isOn);
            _orbits[i].transform.localRotation =
                Quaternion.Euler(0f, config.orbitConfigs[i].swift, 0);
        }
    }

    private int CountBefore(int orbit)
    {
        int total = 0;
        for (int i = 0; i < orbit; i++)
        {
            total += currentConfig.orbitConfigs[i].countBit;
        }
        return total;
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<BitUpgradedSignal>(ShowAll);
    }

    [System.Serializable]
    public class BitOrbitConfig
    {
        public List<OrbitConfig> orbitConfigs = new();
    }

    [System.Serializable]
    public class OrbitConfig
    {
        public int countBit;
        public float swift;
    }
}