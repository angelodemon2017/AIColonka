using System.Collections.Generic;
using UnityEngine;

public abstract class EntityModule : MonoBehaviour
{
    [SerializeField] private List<PointForState> _pointForStates;

    private Dictionary<string, List<Transform>> _cashPointForStates = new();

    internal virtual void Init()
    {
        _pointForStates.ForEach(p => _cashPointForStates.Add(p.state.Key, p.Points));
        _pointForStates.ForEach(p => Debug.LogWarning($"add state key:{p.state.Key}"));
    }

    internal List<Transform> GetPoints(State state)
    {
        if (_cashPointForStates.TryGetValue(state.Key, out List<Transform> result))
        {
            return result;
        }
        return new List<Transform>();
    }
}

[System.Serializable]
public class PointForState
{
    public State state;
    public List<Transform> Points;
}