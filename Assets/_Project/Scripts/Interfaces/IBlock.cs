using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IBlock
    {
        Transform Transform { get; }
        void SetKinematic(bool value);
    }
}