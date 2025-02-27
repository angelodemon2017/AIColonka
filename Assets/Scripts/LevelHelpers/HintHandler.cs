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
    [SerializeField] private MeshRenderer _meshRenderer;

    private float _timeFocused;
    private bool _isRelease = false;

    public string GetHint =>
            $"{ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.BattleBits}/{_needBits}";

    public Transform GetTransform => transform;

    public void Call()
    {
        _isRelease = true;
        _release?.Invoke();
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

            }
        }
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