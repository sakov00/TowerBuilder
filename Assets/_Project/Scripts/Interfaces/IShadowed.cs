using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IShadowed
    {
        Vector3 Position { get; }
        Transform ShadowTransform { get; }
    }
}