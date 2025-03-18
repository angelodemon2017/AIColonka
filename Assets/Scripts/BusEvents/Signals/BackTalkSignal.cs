[System.Serializable]
public class BackTalkSignal
{
    public string Key;
    public float Time;
    public string FromLocalTable;

    public BackTalkSignal(string key, float time, string fromLocalTable)
    {
        Key = key;
        Time = time;
        FromLocalTable = fromLocalTable;
    }
}