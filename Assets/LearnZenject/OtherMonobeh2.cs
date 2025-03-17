using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class OtherMonobeh2 : MonoBehaviour
{
    [Inject] private DiContainer _container;
    [Inject]
    private DialogsConfig _dialogsConfig;
    [Inject]
    private LearnSOInject _learnSOInject;
    [SerializeField] private TestPrefabInject _prefab;

    private void Awake()
    {
        Debug.Log($"2.TestValue:{_learnSOInject.TestValue}");
        Debug.Log($"2.DialogsCn:{_dialogsConfig.dialogs.Count}");

        //        SceneManager.LoadScene("LearnZenject2scene");
        SpawnTestPrefab();
    }

    private void SpawnTestPrefab()
    {
        var prefabInstance = _container.InstantiatePrefabForComponent<TestPrefabInject>(_prefab);
        prefabInstance.Init();
    }
}

public class TestInInject
{
    [Inject]
    [SerializeField] private DialogsConfig _dialogsConfig;
    [Inject]
    [SerializeField] private LearnSOInject _learnSOInject;

    internal DialogsConfig dialogsConfig => _dialogsConfig;
    internal LearnSOInject learnSOInject => _learnSOInject;
}