using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class VirtualObjectChecker : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;

    [SerializeField] private HashSet<IHinter> _hintsH = new();
    
    private IHinter _lastTransform;

    internal IHinter LastHH => _lastTransform;

    private void OnTriggerEnter(Collider other)
    {
        _hintsH.Add(other.GetComponent<IHinter>());
        CheckHints();
    }

    private void OnTriggerExit(Collider other)
    {
        _hintsH.Remove(other.GetComponent<IHinter>());
        CheckHints();
    }

    internal void CheckHints()
    {
        if (_hintsH.Count > 0)
        {
            _lastTransform = _hintsH
                .OrderBy(h => Vector3.Distance(h.GetTransform.position, _checkPoint.position))
                .FirstOrDefault();
        }
        else
        {
            _lastTransform = null;
        }
        _ = ShowHintAsync();
    }

    internal void CallRelease()
    {
        _lastTransform?.Call();
        _hintsH.Remove(_lastTransform);
        _lastTransform = null;
        _ = ShowHintAsync();
    }

    private async Task ShowHintAsync()
    {
        if (WindowGameplay.Instance)
        {
            var totalHint = await GetLocHintAsync();

            WindowGameplay.Instance.SetHintText(totalHint);
        }
    }

    private async Task<string> GetLocHintAsync()
    {
        if (_lastTransform != null)
        {
            var tempHint = await _lastTransform.GetLocHint();

            return $"{_lastTransform.GetKeyForInteract}.{tempHint}";
        }
        return string.Empty;
    }

    private void FixedUpdate()
    {
        if (_hintsH.Count > 1)
        {
            CheckHints();
        }
    }
}