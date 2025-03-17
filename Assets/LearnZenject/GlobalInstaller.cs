using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private DialogsConfig _dialogsConfig;
    [SerializeField] private LearnSOInject _learnSOInject;
    [SerializeField] private TaskConfig _taskConfig;

    public override void InstallBindings()
    {
        Container.Bind<DialogsConfig>().FromScriptableObject(_dialogsConfig).AsSingle();
        Container.Bind<TaskConfig>().FromScriptableObject(_taskConfig).AsSingle();
        Container.Bind<LearnSOInject>().FromScriptableObject(_learnSOInject).AsSingle();

        Container.Bind<ITaskController>().To<TaskController>().AsTransient();

/*        Container.Bind<TaskController>().AsTransient().OnInstantiated((ctx, controller) =>
        {
            var taskConfig = ctx.Container.Resolve<TaskConfig>();
            ((TaskController)controller).InitConfigs(taskConfig);
        });/**/
    }
}