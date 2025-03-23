using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private TaskConfig _taskConfig;
    [SerializeField] private SceneController _sceneController;

    public override void InstallBindings()
    {
        InstallSingletons();
        InstallSinglePrefabs();
        InstallConfigs();
        InstallSignals();

        Container.Bind<SceneController>().FromInstance(_sceneController).AsSingle();
    }

    private void InstallSingletons()
    {
        Container.Bind<DataHandler>().AsSingle();
        Container.Bind<BackTalkHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayHandler>().AsSingle();
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
    }

    private void InstallSignals()
    {        
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<BackTalkSignal>();
        Container.DeclareSignal<BitUpgradedSignal>();
        Container.DeclareSignal<DialogTriggerKeySignal>();
        Container.DeclareSignal<EndBackTalkSignal>();
        Container.DeclareSignal<EnterToSceneSignal>();
        Container.DeclareSignal<ExitFromSceneSignal>();
        Container.DeclareSignal<FocusHintSignal>();
        Container.DeclareSignal<GameModeSignal>();
        Container.DeclareSignal<MetaFightSignal>();
        Container.DeclareSignal<PlayerDamageSignal>();
        Container.DeclareSignal<PlayerDashSignal>();
        Container.DeclareSignal<PlayerHealSignal>();
        Container.DeclareSignal<SetLevelSignal>();
        Container.DeclareSignal<SetNextDialogSignal>();
        Container.DeclareSignal<SetPlayerStateSignal>();
        Container.DeclareSignal<SetTaskSignal>();
        Container.DeclareSignal<SetWindowSignal>();
        Container.DeclareSignal<StartBackTalkSignal>();
        Container.DeclareSignal<TaskUpdatedSignal>();
        Container.DeclareSignal<TransitionSignal>();
        Container.DeclareSignal<WhoInTargetSignal>();
        Container.DeclareSignal<WhoIsDeathSignal>();
    }
}