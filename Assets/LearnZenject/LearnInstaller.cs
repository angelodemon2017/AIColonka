using UnityEngine;
using Zenject;

public class LearnInstaller : MonoInstaller
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private LearnSOInject _learnSOInject;

    public override void InstallBindings()
    {
        //        Container.Bind<DialogsConfig>().FromInstance(_dialogsConfig).AsSingle();
        Container.Bind<DialogsConfig>().FromScriptableObject(_dialogsConfig).AsSingle();
        Container.Bind<LearnSOInject>().FromScriptableObject(_learnSOInject).AsSingle();
        Container.Bind<TestClass>().FromNew().AsSingle();

//        DontDestroyOnLoad(gameObject);
    }
}

public class TestClass
{
    public int TestInt;
}