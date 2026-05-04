using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Build;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class MainBuildingView : BuildView
    {
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }
        [field: SerializeField] public float ProjectileSpeed { get; set; } = 10f;

        public override void Initialize()
        {
        }

        public void SetWalking(bool isWalking)
        {
        }

        public void SetAttacking(bool isAttacking)
        {
            AttackHitEvent?.Invoke();
        }
        
        public Vector3 GetPosition() => _transform.position;
        public Vector3 GetScale() => _transform.localScale;

        public event Action AttackHitEvent;
    }
}