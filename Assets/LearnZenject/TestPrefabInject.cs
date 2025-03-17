using UnityEngine;
using Zenject;

public class TestPrefabInject : MonoBehaviour
{
    [Inject]
    private LearnSOInject _learnSOInject;

/*    [Inject]
    private void Construct(LearnSOInject learnSOInject)
    {
        _learnSOInject = learnSOInject;
    }/**/

    internal void Init()
    {
        Debug.Log($"Init:{_learnSOInject.TestValue}");
    }
}