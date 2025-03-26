[System.Serializable]
public class BackTalkSignal
{
    public string Key;
    public float Time;
    public string FromLocalTable;
    public BackTalkSO BackTalk;

    public BackTalkSignal(string key, float time, string fromLocalTable)
    {
        Key = key;
        Time = time;
        FromLocalTable = fromLocalTable;
    }

    public BackTalkSignal(BackTalkSO backTalk)
    {
        BackTalk = backTalk;
    }
}