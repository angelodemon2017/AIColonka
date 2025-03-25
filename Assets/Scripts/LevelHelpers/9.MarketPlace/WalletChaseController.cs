using UnityEngine;
using Zenject;

public class WalletChaseController : MonoBehaviour
{
    [SerializeField] private FollowerByPoints _followerByPoints;
    [SerializeField] private FollowerByPoints _followerByPointsPhase2;

    [SerializeField] private PointOfFollow _pointOfSecondPhase;
    [SerializeField] private GameObject _chaserRocket;
    [SerializeField] private FlyByRocketState _stateRocketFly;
    [SerializeField] private Transform _pointOnRocket;
    [SerializeField] private Camera _cameraSecondPhase;
    [SerializeField] private SignalAgregator _signalSecondPhase;
    [SerializeField] private SignalAgregator _signalReadyToShoot;
    [SerializeField] private Mover _rocketMover;
    [SerializeField] private InteractionZone _interactByNear;
    [SerializeField] private PositionLerper _positionLerper;
    [SerializeField] private PositionLerper _lookLerper;

    private int counterDrons = 0;
    private bool _chasingOn = false;

    private SignalBus _signalBus;
    private GameplayHandler _gameplayHandler;
    private CameraController _cameraController;

    [Inject]
    private void Construct(
        CameraController cameraController,
        GameplayHandler gameplayHandler,
        SignalBus signalBus)
    {
        _cameraController = cameraController;
        _gameplayHandler = gameplayHandler;
        _signalBus = signalBus;

        _signalBus.Subscribe<WhoIsDeathSignal>(DeathCryptoDron);
    }

    public void Activate()
    {
        _chasingOn = true;
        CheckPhase2();
    }

    private int _speedRocket = 1;

    public void AddSpeed(bool isAdd)
    {
        _speedRocket += isAdd ? 1 : -1;
        _speedRocket = Mathf.Clamp(_speedRocket, 1, 10);
        CheckPhase2();
    }

    public void InterActPhase2()
    {
        _speedRocket = 2;
        CheckPhase2();
    }

    private void CheckPhase2()
    {
        _rocketMover.SetSpeed(_speedRocket);
        _interactByNear.gameObject.SetActive(_speedRocket > 4);
        if (_speedRocket > 4)
        {
            _signalReadyToShoot.FireAll(_signalBus);
        }
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        _pointOnRocket.localRotation = Quaternion.Euler(0f, horizontal * 20f, 0f);
    }

    private void DeathCryptoDron()
    {
        if (!_chasingOn)
        {
            return;
        }

        counterDrons++;
        if (counterDrons == 2)
        {
            _signalBus.Fire(new TransitionSignal(false, StartSecondPhase));
        }
    }

    private void StartSecondPhase()
    {
        SetPlayerOnRocket();
        _chaserRocket.SetActive(true);

        _followerByPoints.gameObject.SetActive(false);
        _followerByPointsPhase2.gameObject.SetActive(true);

        var newInst = Instantiate(_stateRocketFly);
        newInst.CustomInit(_positionLerper, _lookLerper);
        _signalBus.Fire(new SetPlayerStateSignal(newInst, true));

        _signalBus.Fire(new TransitionSignal(true, () =>
            _signalSecondPhase.FireAll(_signalBus)));
    }

    [ContextMenu("SetPlayerOnRocket")]
    private void SetPlayerOnRocket()
    {
        _cameraController.SwitchCamera(_cameraSecondPhase);
        _gameplayHandler.PlayerInstance.GetFallingController.SwitchGravity();
        _gameplayHandler.PlayerInstance.transform.position = _pointOnRocket.position;
        _gameplayHandler.PlayerInstance.transform.SetParent(_pointOnRocket);
        _gameplayHandler.PlayerInstance.transform.rotation = _pointOnRocket.rotation * Quaternion.Euler(0f, -120f, 0f);
    }

    [ContextMenu("ResetRocket")]
    private void ResetRocket()
    {
        _cameraController.ResetCamera();
        _gameplayHandler.PlayerInstance.transform.SetParent(null);
        _gameplayHandler.PlayerInstance.GetFallingController.ResetFalling();
        _gameplayHandler.PlayerInstance.transform.position = Vector3.zero;
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<WhoIsDeathSignal>(DeathCryptoDron);
    }
}