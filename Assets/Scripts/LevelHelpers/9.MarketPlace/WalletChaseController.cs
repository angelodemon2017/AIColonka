using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WalletChaseController : MonoBehaviour
{
    [SerializeField] private FollowerByPoints _followerByPoints;
    [SerializeField] private FollowerByPoints _followerByPointsPhase2;
    [SerializeField] private List<HPComponent> _drons;

    [SerializeField] private PointOfFollow _pointOfSecondPhase;
    [SerializeField] private GameObject _chaserRocket;
    [SerializeField] private FlyByRocketState _stateRocketFly;
    [SerializeField] private Transform _pointOnRocket;
    [SerializeField] private Camera _cameraSecondPhase;
    [SerializeField] private SignalAgregator _signalSecondPhase;
    [SerializeField] private List<SignalAgregator> _signalReadyToShoot;
    [SerializeField] private SignalAgregator _lastOneDrone;
    [SerializeField] private Mover _rocketMover;
    [SerializeField] private InteractionZone _interactByNear;
    [SerializeField] private InteractionZone _interactFinalChasing;
    [SerializeField] private PositionLerper _positionLerper;
    [SerializeField] private PositionLerper _lookLerper;
    [SerializeField] private PositionLerper _rocketLerper;
    [SerializeField] private PositionLerper _cameraLerper;
    [SerializeField] private BitShooter _bitWeapon;

    [SerializeField] private PlayerState _returningState;
    [SerializeField] private GameObject _walletFinal;
    [SerializeField] private Transform _pointNearFinalWallet;

    private int _tries = 0;
    private int counterDrons = 0;
    private bool _chasingOn = false;

    private DiContainer _diContainer;
    private SignalBus _signalBus;
    private GameplayHandler _gameplayHandler;
    private CameraController _cameraController;

    [Inject]
    private void Construct(
        DiContainer diContainer,
        CameraController cameraController,
        GameplayHandler gameplayHandler,
        SignalBus signalBus)
    {
        _diContainer = diContainer;
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
        _tries++;
        _speedRocket = 2;
        CheckPhase2();
        _gameplayHandler.PlayerInstance.BitsController.SetBits(false);
        var w = _diContainer.InstantiatePrefabForComponent<BitShooter>(_bitWeapon);
        if (_tries > 2)
        {
            w.CruiseActivate();
        }
        w.Init(EnumWhoIs.Player,
            _gameplayHandler.PlayerInstance.GetPoints.PointOfLookCamera,
            null,
            _cameraController.GetTransform.rotation);
    }

    [ContextMenu("InteractFinal")]
    public void InteractFinal()
    {
        _positionLerper.transform.localPosition = Vector3.zero;
        _rocketMover.SetSpeed(10);
        _signalBus.Fire(new TransitionSignal(false, TransToFinalScene));
    }

    private void TransToFinalScene()
    {
        _interactFinalChasing.gameObject.SetActive(false);
        _followerByPointsPhase2.gameObject.SetActive(false);
        _chaserRocket.gameObject.SetActive(false);

        _walletFinal.SetActive(true);
        _signalBus.Fire(new SetPlayerStateSignal(_returningState));

        PlayerReset();

        _gameplayHandler.PlayerInstance.BitsController.SetBits(true);
        _signalBus.Fire(new TransitionSignal(true));
    }

    private void CheckPhase2()
    {
        _rocketMover.SetSpeed(_speedRocket);
        _interactByNear.gameObject.SetActive(_speedRocket > 4);
        if (_speedRocket > 4)
        {
            _signalReadyToShoot.GetBorderElement(_tries)
                .FireAll(_signalBus);
        }
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

    public void DeathDronInChasing()
    {
        _lastOneDrone.FireAll(_signalBus);
        _interactByNear.enabled = false;
        _interactFinalChasing.gameObject.SetActive(true);
    }

    private void StartSecondPhase()
    {
        SetPlayerOnRocket();
        _chaserRocket.SetActive(true);

        _followerByPoints.gameObject.SetActive(false);
        _followerByPointsPhase2.gameObject.SetActive(true);

        var newInst = Instantiate(_stateRocketFly);
        newInst.CustomInit(_lookLerper, _rocketLerper, _cameraLerper);
        _signalBus.Fire(new SetPlayerStateSignal(newInst, true));

        _signalBus.Fire(new TransitionSignal(true, () =>
            _signalSecondPhase.FireAll(_signalBus)));

        _drons.ForEach(d => d.Kill());
    }

    [ContextMenu("SetPlayerOnRocket")]
    private void SetPlayerOnRocket()
    {
        _cameraController.SwitchCamera(_cameraSecondPhase);
        _gameplayHandler.PlayerInstance.GetFallingController.SwitchOffGravity();
        _gameplayHandler.PlayerInstance.transform.position = _pointOnRocket.position;
        _gameplayHandler.PlayerInstance.transform.SetParent(_pointOnRocket);
        _gameplayHandler.PlayerInstance.transform.rotation = _pointOnRocket.rotation * Quaternion.Euler(0f, -120f, 0f);
    }

    [ContextMenu("PlayerReset")]
    private void PlayerReset()
    {
        _cameraController.ResetCamera();
        _gameplayHandler.PlayerInstance.gameObject.SetActive(false);

        _gameplayHandler.PlayerInstance.transform.SetParent(_pointNearFinalWallet);
        _gameplayHandler.PlayerInstance.transform.localPosition = Vector3.zero;
        _gameplayHandler.PlayerInstance.transform.rotation = _pointNearFinalWallet.rotation;

        _gameplayHandler.PlayerInstance.GetFallingController.ResetFalling();
        _gameplayHandler.PlayerInstance.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<WhoIsDeathSignal>(DeathCryptoDron);
    }
}