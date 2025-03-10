using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheatsPanel : MonoBehaviour
{
    [SerializeField] private Button _addBitBTN;
    [SerializeField] private Button _remBitBTN;
    [SerializeField] private Button _addWVBTN;
    [SerializeField] private Button _remWVBTN;
    [SerializeField] private TextMeshProUGUI _bits;
    [SerializeField] private TextMeshProUGUI _wvs;

    private MainData _mainData => ControllerDemoSaveFile.Instance.mainData;

    private void Awake()
    {
        _addBitBTN.onClick.AddListener(BitsAdd);
        _remBitBTN.onClick.AddListener(BitsRem);
        _addWVBTN.onClick.AddListener(WVAdd);
        _remWVBTN.onClick.AddListener(WVRem);

        UpdateUI();
    }

    private void BitsAdd()
    {
        _mainData.AddBits(1);
        UpdateUI();
    }

    private void BitsRem()
    {
        _mainData.AddBits(-1);
        UpdateUI();
    }

    private void WVAdd()
    {
        _mainData.AddAVP(1);
        UpdateUI();
    }

    private void WVRem()
    {
        _mainData.AddAVP(-1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _bits.text = $"Bits:({_mainData.gamePlayProgress.BattleBits})";
        _wvs.text = $"WV:({_mainData.gamePlayProgress.AVPower})";
    }

    private void OnDestroy()
    {
        _addBitBTN.onClick.RemoveAllListeners();
        _remBitBTN.onClick.RemoveAllListeners();
        _addWVBTN.onClick.RemoveAllListeners();
        _remWVBTN.onClick.RemoveAllListeners();
    }
}