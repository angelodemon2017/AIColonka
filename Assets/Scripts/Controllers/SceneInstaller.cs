using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private RoomMapping _roomMapping;

    public override void InstallBindings()
    {
        if (_cameraController)
        {
            Container.Bind<CameraController>().FromComponentInHierarchy(_cameraController).AsSingle();
        }
        if (_roomMapping)
        {
            Container.Inject(_roomMapping);
        }
    }
}