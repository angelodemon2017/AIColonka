using System.Threading.Tasks;
using UnityEngine;

public interface IHinter
{
    string GetKeyForInteract => KeyCode.E.ToString();
    Transform GetTransform { get; }

    Task<string> GetLocHint();
    void Call();
}