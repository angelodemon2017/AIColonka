using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private LearnSOInject _learnSOInject;
    [SerializeField] private TaskConfig _taskConfig;

    public override void InstallBindings()
    {
        InstallSingletons();
        InstallConfigs();
        InstallSignals();
    }

    private void InstallSingletons()
    {
        Container.Bind<DataHandler>().AsSingle();
    }

    private void InstallConfigs()
    {
        Container.Bind<DialogsConfig>().FromScriptableObject(_dialogsConfig).AsSingle();
        Container.Bind<TaskConfig>().FromScriptableObject(_taskConfig).AsSingle();

        //demo:
        Container.Bind<LearnSOInject>().FromScriptableObject(_learnSOInject).AsSingle();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<SetTaskSignal>();

        //demo:
        Container.DeclareSignal<DemoSignal>();
        Container.DeclareSignal<ShowSignal>();
    }
}