using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [SerializeField] private List<BitOrbit> _orbits = new();
    [SerializeField] private List<BitOrbitConfig> _bitOrbitConfigs = new();

    [SerializeField] private Material _peaceBitMaterial;
    [SerializeField] private Material _fightBitMaterial;

    private BitOrbitConfig currentConfig => _bitOrbitConfigs[ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits];
    private bool isFighing => EntityRepository.Instance.HaveEnemies();

    private void Awake()
    {
        SetBits(true);
    }

/*    internal void Show(bool isShow = true)
    {
        var config = currentConfig;

        //        _orbits.ForEach(o => o.gameObject.SetActive(isShow));

        for (int i = 0; i < 3; i++)
        {
            _orbits[i].transform.localRotation =
                Quaternion.Euler(0f, config.orbitConfigs[i].swift, 0);// _orbits[i].transform.parent.localRotation.eulerAngles.z);
        }
        _orbits[0].OnOffBits(config.orbitConfigs[0].countBit, 0.1f, isShow);
        _orbits[1].OnOffBits(config.orbitConfigs[1].countBit, 
            0.1f * config.orbitConfigs[0].countBit, isShow);
        _orbits[2].OnOffBits(config.orbitConfigs[2].countBit,
            0.1f * (config.orbitConfigs[0].countBit + config.orbitConfigs[1].countBit),
            isShow);
    }/**/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits--;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits, 0, 9);
            SetBits(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits++;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits, 0, 9);
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