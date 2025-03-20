using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private UIFSM _uifsm;
    [SerializeField] private RoomMapping _roomMapping;

    public override void InstallBindings()
    {
        if (_cameraController)
        {
            Container.Bind<CameraController>().FromComponentInHierarchy(_cameraController).AsSingle();
        }
        Container.Bind<UIFSM>().FromComponentInHierarchy(_uifsm).AsSingle();
        if (_roomMapping)
        {
            Container.Inject(_roomMapping);
        }
        //        Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
    }
}