using UnityEngine;

[CreateAssetMenu(menuName = "SO/TaskSO", order = 1)]
public class TaskSO : ScriptableObject
{
    public string Name;
    public string Description;

    public string GetDescription => Description;
}