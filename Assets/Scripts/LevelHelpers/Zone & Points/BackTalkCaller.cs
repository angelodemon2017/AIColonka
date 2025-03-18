using UnityEngine;
using Zenject;

public class BackTalkCaller : MonoBehaviour
{
    [SerializeField] private string _keyTalk;
    [SerializeField] private float _showTime;

    [Inject] private SignalBus _signalBus;

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1.5f, $"BackTalkCaller:{_keyTalk}");
    }

    public void CallTalk()
    {
        _signalBus.Fire(new BackTalkSignal(_keyTalk, _showTime, Localizations.Tables.BackTalksTable));
//        _ = ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(_keyTalk, _showTime, Localizations.Tables.BackTalksTable);
    }
}