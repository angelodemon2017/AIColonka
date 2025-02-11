using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// May be delete
/// </summary>
public class InputFSM : MonoBehaviour
{
    [SerializeField] private List<KeyMap> _pareKeyEvents = new();
    private Dictionary<KeyCode, EnumUIEvent> _tempMapKeyEvents = new();

    private void Awake()
    {
        InitMapKey();
    }

    private void InitMapKey()
    {
        _tempMapKeyEvents.Clear();
        _pareKeyEvents.ForEach(p => _tempMapKeyEvents.Add(p.keyCode, p.uIEvent));
    }

    [System.Serializable]
    internal class KeyMap
    {
        internal KeyCode keyCode;
        internal EnumUIEvent uIEvent;
    }
}