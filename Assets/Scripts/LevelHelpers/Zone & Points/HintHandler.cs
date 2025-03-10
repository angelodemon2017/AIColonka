using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class HintHandler : MonoBehaviour, IHinter
{
    [Range(1, 8)]
    [SerializeField] private int _needBits;
    [SerializeField] private UnityEvent _release;
    [SerializeField] private string _keyHint;
    [SerializeField] private BackTalkCaller _backTalkByFall;

    private float _timeFocused;
    private bool _isRelease = false;

    public string GetHint =>
            $"{ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits}/{_needBits}";

    public Transform GetTransform => transform;
    private bool AvailableCall => ControllerDemoSaveFile.Instance.IsDebug || ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits >= _needBits;

    public void Call()
    {
        if (AvailableCall)
        {
            _isRelease = true;
            _release?.Invoke();
        }
        else
        {
            _backTalkByFall?.CallTalk();
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