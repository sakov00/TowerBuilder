using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IAttackable
    {
        Vector3 Position { get; }
        float AttackRange { get; }
        void SetAttacking(bool isAttacking);
        Vector3 AttackPoint();
    }
}