using System.Threading.Tasks;
using UnityEngine;

public interface IHinter
{
    string GetKeyForInteract => KeyCode.E.ToString();
    bool AvailableCall { get; }
    Transform GetTransform { get; }

    void InFocus();
    Task<string> GetLocHint();
    void Call();
}