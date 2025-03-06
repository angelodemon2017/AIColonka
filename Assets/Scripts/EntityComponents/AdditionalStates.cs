using UnityEngine;

public class AdditionalStates : MonoBehaviour
{
    private bool _isSlower;

    internal bool IsSlow => _isSlower;

    internal void SetSlow(bool isSlow)
    {
        _isSlower = isSlow;
    }
}