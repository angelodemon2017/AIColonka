using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [SerializeField] private List<BitOrbit> _orbits = new();
    [SerializeField] private List<BitOrbitConfig> _bitOrbitConfigs = new();

    [SerializeField] private Material _peaceBitMaterial;
    [SerializeField] private Material _fightBitMaterial;

    private BitOrbitConfig currentConfig => _bitOrbitConfigs[currentBit];
    private bool isFighing => EntityRepository.Instance.HaveEnemies();
    private int currentBit => ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits;

    private void Awake()
    {
        ControllerDemoSaveFile.Instance.mainData.BitUpgrade += ShowAll;
    }

    private void Start()
    {
        ShowAll();
    }

    private void ShowAll()
    {
        SetBits(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits--;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(currentBit, 0, 9);
            SetBits(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits++;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(currentBit, 0, 9);
            SetBits(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetBits(true);
        }
    }

    internal void UpdateMode()
    {
        var isf = isFighing;
        foreach (var o in _orbits)
        {
            o.Rotator.SetSpeed(isf ? 5f : 2f);
            o.SetMaterial(isf ? _fightBitMaterial : _peaceBitMaterial);
        }
    }

    internal void SetBits(bool isOn)
    {
        Debug.Log($"show bits:{currentBit}");
        var config = currentConfig;
        for (int i = 0; i < 3; i++)
        {
            _orbits[i].SetBits(config.orbitConfigs[i].countBit, CountBefore(i), isOn);
            _orbits[i].transform.localRotation =
                Quaternion.Euler(0f, config.orbitConfigs[i].swift, 0);// _orbits[i].transform.parent.localRotation.eulerAngles.z);
        }
    }/**/

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
        ControllerDemoSaveFile.Instance.mainData.BitUpgrade -= ShowAll;
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