using UnityEngine;

[CreateAssetMenu(menuName = "SO/BackTalk", order = 1)]
public class BackTalkSO : ScriptableObject
{
    const string DEFTABLE = "BackTalks";

    public EnumChapter Chapter;
    public string KeyLocal;
    public float Seconds;
    public SignalAgregator SignalAfterEnd;

    public string Table => DEFTABLE;
}