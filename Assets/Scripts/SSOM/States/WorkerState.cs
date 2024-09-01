using UnityEngine;

[CreateAssetMenu]
public class WorkerState : State
{
    private GenerationPlatform _generationPlatform;
    private Transform _storeObject;
    private GameObject _keepingObject;

    protected override void Init()
    {
        _generationPlatform = GameObject.Find("GenerationObjects").GetComponent<GenerationPlatform>();
        _storeObject = GameObject.Find("StoreObjects").transform;

    }

    protected override void Run()
    {

    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}