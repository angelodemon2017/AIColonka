using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private TaskConfig taskConfig;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

//        SceneManager.LoadSceneAsync(1);
    }

    public override void InstallBindings()
    {
        //        Container.Bind<BulletPool>().ToSelf().AsSingle().Initializable();
        Container.Bind<TaskConfig>().FromInstance(taskConfig);//.AsSingle();//.NonLazy();
//        Container.Bind<TaskConfig>().FromInstance(taskConfig).AsTransient();

//        Container.Bind<ControllerDemoSaveFile>().AsTransient();

        SceneManager.LoadSceneAsync(1);
    }
}