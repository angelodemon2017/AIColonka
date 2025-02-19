using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private List<Transform> _bits = new();
    [SerializeField] private GameObject _bit;
    [SerializeField] private Transform _parent;
    [SerializeField] private float _speedRotate;
    [SerializeField] private BitScript _bitScript;
    [SerializeField] private float _speedShoot;
    [SerializeField] private List<BitOrbit> _orbits = new();
    [SerializeField] private List<BitOrbitConfig> _bitOrbitConfigs = new();

    [SerializeField] private Material _peaceBitMaterial;
    [SerializeField] private Material _fightBitMaterial;

    private int _valueDamage = 1;
    private Damage damage => new Damage(EnumDamageType.BitRange, _valueDamage);
    private bool isFighing => EntityRepository.Instance.HaveEnemies();

    private void Awake()
    {
        SetBits();
    }

    internal void Show(bool isShow = true)
    {
        _orbits.ForEach(o => o.gameObject.SetActive(isShow));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits--;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits, 0, 9);
            SetBits();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits++;
            ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits =
                Mathf.Clamp(ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits, 0, 9);
            SetBits();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetBits();
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

    private void SetBits()
    {
        var config = _bitOrbitConfigs[ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits];
        for (int i = 0; i < 3; i++)
        {
            _orbits[i].SetBits(config.orbitConfigs[i].countBit);
            _orbits[i].transform.localRotation =
                Quaternion.Euler(0f, config.orbitConfigs[i].swift, 0);// _orbits[i].transform.parent.localRotation.eulerAngles.z);
        }
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