using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class InteractionZone : MonoBehaviour, IHinter
{
    [SerializeField] private UnityEvent _interact;
    [SerializeField] private string keyHint;

    public async Task<string> GetLocHint()
    {
        return await Localizations.GetLocalizedText(
            Localizations.Tables.GamePlay, keyHint);
    }

    public Transform GetTransform => transform;

    public void Call()
    {
        _interact?.Invoke();
    }
}