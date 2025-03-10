using UnityEngine;

public class PointMover : MonoBehaviour
{
    [SerializeField] private PlayerDashState _playerDashState;

    public void MovePlayer()
    {
        var ps = Instantiate(_playerDashState);
        ps.SetCustomTargetPoint(transform.position);
        PlayerFSM.Instance.SetPreparedState(ps);
    }
}