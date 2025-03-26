using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "SO/TaskSO", order = 1)]
public class TaskSO : ScriptableObject, ICachable<string>
{
    public string KeyTitle;
    public string KeyLocDesc;
    public List<EnumLevels> AvailableLevels;
    [SerializeField] private EnumLevels _whereTarget;

    public LocalizedString _locaString;

    internal EnumLevels GetTargetLvl => _whereTarget;
    public string GetKey => KeyTitle;

    internal async Task<string> GetTitle()
    {
        var locResult = await Localizations.GetLocalizedText(
            Localizations.Tables.Tasks, KeyTitle);

        if (locResult == KeyTitle)
        {
            locResult = name;
        }

        return locResult;
    }
    internal async Task<string> GetDescription()
    {
        var locResult = await Localizations.GetLocalizedText(
            Localizations.Tables.Tasks, KeyLocDesc);

        if (locResult == KeyLocDesc)
        {
            locResult = name;
        }

        return locResult;
    }

    public void UpdateLocKeys()
    {

    }
}