using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OtherMonobeh : MonoBehaviour
{
    private DialogsConfig _dialogsConfig;
    private LearnSOInject _learnSOInject;

    [Inject]
    private void Olala(LearnSOInject learnSOInject)
    {
        _learnSOInject = learnSOInject;
    }

    private void Awake()
    {
        Debug.Log($"1.TestValue:{_learnSOInject.TestValue}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"1.TestValue:{_learnSOInject.TestValue}");
        }
    }
}