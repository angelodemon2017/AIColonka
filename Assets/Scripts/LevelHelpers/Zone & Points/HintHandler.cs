using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class HintHandler : MonoBehaviour, IHinter
{
    [Range(1, 8)]
    [SerializeField] private int _needBits;
    [SerializeField] private UnityEvent _release;
    [SerializeField] private string _keyHint;
    private const string LOWBITS = "0.LOWBITS";

    private SignalBus _signalBus;
    private DataHandler _dataHandler;
    private float _timeFocused;
    private bool _isRelease = false;

    public string GetHint =>
            $"{_dataHandler.CurrentData.gamePlayProgress.BattleBits}/{_needBits}";

    public Transform GetTransform => transform;
    private bool AvailableCall => _dataHandler.Settings.IsDebug || _dataHandler.CurrentData.gamePlayProgress.BattleBits >= _needBits;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        DataHandler dataHandler)
    {
        _signalBus = signalBus;
        _dataHandler = dataHandler;
    }

    public void Call()
    {
        if (AvailableCall)
        {
            _isRelease = true;
            _release?.Invoke();
        }
        else
        {
            _signalBus.Fire(new BackTalkSignal(LOWBITS, 3f, Localizations.Tables.BackTalksTable));
        }
    }

    public async Task<string> GetLocHint()
    {
        return await Localizations.GetLocalizedText(
            Localizations.Tables.GamePlay, _keyHint) + GetHint;
    }

    private void FixedUpdate()
    {
        if (_timeFocused > 0f)
        {
            _timeFocused -= Time.fixedDeltaTime;
            if (_timeFocused <= 0)
            {
                //unfocus
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 3f, $"HintHandler.B:{_needBits}");
    }

    public void InFocus()
    {
        if (!_isRelease)
        {
            _timeFocused = 0.1f;
//          some focused
        }
    }
}