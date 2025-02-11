using System.Collections.Generic;
using UnityEngine;

public class PanelSelectingLevel : MAINWindow
{
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;

    [SerializeField] private List<EnumLevels> _availableLevels;

    private IEnumerable<EnumLevels> AvailableLevels => _availableLevels;

    private void Awake()
    {
        InitButtons();
//        EventBus.Subscribe<EventKey>(CheckKey);
    }

    private void InitButtons()
    {
        _parentButtons.DestroyChildrens();
        foreach (var lev in AvailableLevels)
        {
            var newLev = Instantiate(_prefabLevelButtonPresent, _parentButtons);
            newLev.Init(lev);
        }
    }

/*    private void CheckKey(EventKey eventKey)
    {
        if (eventKey.pressedKey == KeyCode.L)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }/**/

    private void OnDestroy()
    {
//        EventBus.Unsubscribe<EventKey>(CheckKey);
    }
}