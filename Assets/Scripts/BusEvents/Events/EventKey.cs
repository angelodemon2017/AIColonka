using UnityEditor;
using UnityEngine;

public class EventKey : IEvent
{
    internal KeyCode pressedKey;

    internal EventKey(KeyCode key)
    {
        pressedKey = key;
    }
}