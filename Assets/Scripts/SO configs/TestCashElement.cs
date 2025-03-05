using UnityEngine;

[CreateAssetMenu(menuName = "SO/TestCashElement", order = 1)]
public class TestCashElement : ScriptableObject, ICachable<string>
{
    public string Key;

    public int SomeField;

    public string GetKey => Key;
}