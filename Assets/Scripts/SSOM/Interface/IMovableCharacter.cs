using UnityEngine;
using UnityEngine.AI;

public interface IMovableCharacter
{
    void GoToPoint(Vector3 target);

    NavMeshAgent GetNavMeshAgent();
}