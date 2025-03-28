using System.Collections.Generic;
using UnityEngine;

public class AlphaSphereController : MonoBehaviour
{
    private const string RADIUS_SPHERE = "_RadiusSphere";
    private const string CENTER_SPHERE = "_CenterSphere";

    [SerializeField] private List<Material> _materials;
    [SerializeField] private float _radius;

    private void OnValidate()
    {
        SetRadius(_radius);
    }

    internal void SetCenter(Vector3 vector)
    {
        _materials.ForEach(m => m.SetVector(CENTER_SPHERE, vector));
    }

    internal void SetRadius(float rad)
    {
        _materials.ForEach(m => m.SetFloat(RADIUS_SPHERE, rad));
    }
}