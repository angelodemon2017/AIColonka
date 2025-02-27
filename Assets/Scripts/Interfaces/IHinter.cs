using System.Threading.Tasks;
using UnityEngine;

public interface IHinter
{
    string GetKeyForInteract => KeyCode.E.ToString();
    Transform GetTransform { get; }

    void InFocus();
    Task<string> GetLocHint();
    void Call();
}