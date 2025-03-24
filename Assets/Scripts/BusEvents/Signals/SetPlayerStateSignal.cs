using System;

[Serializable]
public class SetPlayerStateSignal
{
    public PlayerState _playerState;
    public bool IsWasInstanting;

    public SetPlayerStateSignal(PlayerState playerState, bool instanting = false)
    {
        _playerState = playerState;
        IsWasInstanting = instanting;
    }
}