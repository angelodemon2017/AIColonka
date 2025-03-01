using UnityEngine;

public class PlayerStateCustomSetter : MonoBehaviour
{
    [SerializeField] private PlayerState _playerState;

    public void SetState()
    {
        PlayerFSM.Instance.SetState(_playerState);
    }
}