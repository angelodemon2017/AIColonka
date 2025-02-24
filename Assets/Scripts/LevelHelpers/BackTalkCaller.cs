using UnityEngine;

public class BackTalkCaller : MonoBehaviour
{
    [SerializeField] private string _keyTalk;
    [SerializeField] private float _showTime;

    public void CallTalk()
    {
        ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(_keyTalk, _showTime);
    }
}