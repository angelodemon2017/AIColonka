using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "SO/TaskSO", order = 1)]
public class TaskSO : ScriptableObject, ICachable<string>
{
    public string KeyTitle;
    public string KeyLocDesc;
    public List<EnumLevels> AvailableLevels;

    public LocalizedString _locaString;

    public string GetKey => KeyTitle;

    public void UpdateLocKeys()
    {

    }
}