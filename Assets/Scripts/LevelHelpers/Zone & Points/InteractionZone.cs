using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class InteractionZone : MonoBehaviour, IHinter
{
    [SerializeField] private UnityEvent _interact;
    [SerializeField] private string keyHint;
    [SerializeField] private SignalAgregator _signalAgregator;

    [Inject] private SignalBus _signalBus;

    public async Task<string> GetLocHint()
    {
        return await Localizations.GetLocalizedText(
            Localizations.Tables.GamePlay, keyHint);
    }

    public Transform GetTransform => transform;

    public void Call()
    {
        _signalAgregator.FireAll(_signalBus);
        _interact?.Invoke();
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1f, $"InteractionZone:(E.)");
    }

    public void InFocus()
    {

    }
}