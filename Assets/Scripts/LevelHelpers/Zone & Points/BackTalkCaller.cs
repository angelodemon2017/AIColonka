using UnityEngine;

public class BackTalkCaller : MonoBehaviour
{
    [SerializeField] private string _keyTalk;
    [SerializeField] private float _showTime;

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1.5f, $"BackTalkCaller:{_keyTalk}");
    }

    public void CallTalk()
    {
        _ = ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(_keyTalk, _showTime, Localizations.Tables.BackTalksTable);
    }
}