using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "SO/TaskSO", order = 1)]
public class TaskSO : ScriptableObject
{
    public string TaskKey;
    public string AddedKey;
    public string KeyTitle;
    public string KeyLocDesc;
    public List<EnumLevels> AvailableLevels;

//    public LocalizedStringTable _locTable;
    public LocalizedString _locaString;

    public void UpdateLocKeys()
    {

    }
}