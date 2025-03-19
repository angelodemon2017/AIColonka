using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private LearnSOInject _learnSOInject;
    [SerializeField] private TaskConfig _taskConfig;

    public override void InstallBindings()
    {
        InstallSingletons();
        InstallSinglePrefabs();
        InstallConfigs();
        InstallSignals();

        SceneManager.LoadSceneAsync(1);
    }

    private void InstallSingletons()
    {        
        Container.Bind<DataHandler>().AsSingle();
        Container.Bind<BackTalkHandler>().AsSingle();
    }

    private void InstallSinglePrefabs()
    {/*example for future
        Container.Bind<WindowGameplay>()
                 .FromComponentInNewPrefab(_windowGameplay)
                 .AsSingle();/**/
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
        Container.DeclareSignal<BitUpgradedSignal>();
        Container.DeclareSignal<EndBackTalkSignal>();
        Container.DeclareSignal<PlayerDeathSignal>();
        Container.DeclareSignal<RestartLevelSignal>();
        Container.DeclareSignal<SetLevelSignal>();
        Container.DeclareSignal<SetNextDialogSignal>();
        Container.DeclareSignal<SetPlayerStateSignal>();
        Container.DeclareSignal<SetTaskSignal>();
        Container.DeclareSignal<SetWindowSignal>();
        Container.DeclareSignal<StartBackTalkSignal>();
        Container.DeclareSignal<TaskUpdatedSignal>();        

        //demo:
        Container.DeclareSignal<ShowSignal>();
    }
}