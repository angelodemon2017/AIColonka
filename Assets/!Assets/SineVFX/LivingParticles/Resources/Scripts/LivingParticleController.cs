using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LivingParticleController : MonoBehaviour {

    public Transform affector;

    private ParticleSystemRenderer psr;

    private GameplayHandler _gameplayHandler;

    [Inject]
    private void Construct(GameplayHandler gameplayHandler)
    {
        _gameplayHandler = gameplayHandler;
    }

	void Start () {
        psr = GetComponent<ParticleSystemRenderer>();
        affector = _gameplayHandler.PlayerInstance.transform;
    }

	void Update () {
        psr.material.SetVector("_Affector", affector.position);
    }
}
