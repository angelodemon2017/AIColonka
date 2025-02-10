using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelSelectingLevel : MonoBehaviour
{
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;

    [SerializeField] private List<EnumLevels> _availableLevels;

    private IEnumerable<int> AvailableLevels => _availableLevels.OfType<int>();

    private void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        foreach (var lev in AvailableLevels)
        {
            var newLev = Instantiate(_prefabLevelButtonPresent, _parentButtons);
            newLev.Init(lev);
        }
    }
}