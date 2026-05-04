using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IMovable
    {
        Vector3 Position { get; }
        bool IsMoving { get; }
        void MoveTo(Vector3 point);
        void Stop();
    }
}