using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class HintHandler : MonoBehaviour, IHinter
{
    [Range(1, 8)]
    [SerializeField] private int _needBits;
    [SerializeField] private UnityEvent _release;
    [SerializeField] private bool _keyIsRorE;
    [SerializeField] private string _keyHint;

    public string GetHint =>
            $"{ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits}/{_needBits}";

    public Transform GetTransform => transform;

    public void Call()
    {
        _release?.Invoke();
    }

    public async Task<string> GetLocHint()
    {
        return await Localizations.GetLocalizedText(
            Localizations.Tables.GamePlay, _keyHint) + GetHint;
    }
}