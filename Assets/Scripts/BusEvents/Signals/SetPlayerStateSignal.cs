using System;

[Serializable]
public class SetPlayerStateSignal
{
    public PlayerState _playerState;

    public SetPlayerStateSignal(PlayerState playerState)
    {
        _playerState = playerState;
    }
}