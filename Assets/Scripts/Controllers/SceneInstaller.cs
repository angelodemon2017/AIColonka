using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private CameraController _cameraController;

    public override void InstallBindings()
    {
        Container.Bind<CameraController>().FromComponentInHierarchy(_cameraController).AsSingle();
//        Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
    }
}