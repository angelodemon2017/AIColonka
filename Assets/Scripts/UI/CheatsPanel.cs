using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class CheatsPanel : MonoBehaviour
{
    [SerializeField] private Button _addBitBTN;
    [SerializeField] private Button _remBitBTN;
    [SerializeField] private Button _addWVBTN;
    [SerializeField] private Button _remWVBTN;
    [SerializeField] private TextMeshProUGUI _bits;
    [SerializeField] private TextMeshProUGUI _wvs;

    private DataHandler _dataHandler;

    [Inject]
    private void Construct(
        DataHandler dataHandler)
    {
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        UpdateUI();
    }

    private void Awake()
    {
        _addBitBTN.onClick.AddListener(BitsAdd);
        _remBitBTN.onClick.AddListener(BitsRem);
        _addWVBTN.onClick.AddListener(WVAdd);
        _remWVBTN.onClick.AddListener(WVRem);
    }

    private void BitsAdd()
    {
        _dataHandler.AddBits(1);
        UpdateUI();
    }

    private void BitsRem()
    {
        _dataHandler.AddBits(-1);
        UpdateUI();
    }

    private void WVAdd()
    {
        _dataHandler.AddWVs(1);
        UpdateUI();
    }

    private void WVRem()
    {
        _dataHandler.AddWVs(-1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _bits.text = $"Bits:({_dataHandler.CurrentData.gamePlayProgress.BattleBits})";
        _wvs.text = $"WV:({_dataHandler.CurrentData.gamePlayProgress.AVPower})";
    }

    private void OnDestroy()
    {
        _addBitBTN.onClick.RemoveAllListeners();
        _remBitBTN.onClick.RemoveAllListeners();
        _addWVBTN.onClick.RemoveAllListeners();
        _remWVBTN.onClick.RemoveAllListeners();
    }
}