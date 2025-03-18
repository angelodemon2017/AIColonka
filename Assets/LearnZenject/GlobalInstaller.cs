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
        Container.Bind<BackTalkHandler>().AsSingle();
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
        Container.DeclareSignal<BackTalkSignal>();
        Container.DeclareSignal<EndBackTalkSignal>();
        Container.DeclareSignal<SetNextDialogSignal>();
        Container.DeclareSignal<SetRoomPresetSignal>();
        Container.DeclareSignal<SetTaskSignal>();
        Container.DeclareSignal<SetWindowSignal>();
        Container.DeclareSignal<TaskUpdatedSignal>();        

        //demo:
        Container.DeclareSignal<DemoSignal>();
        Container.DeclareSignal<ShowSignal>();
    }
}