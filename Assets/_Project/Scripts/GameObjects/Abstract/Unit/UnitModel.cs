using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(WarriorModel))]
    [MemoryPackUnion(1, typeof(ArcherModel))]
    [MemoryPackUnion(2, typeof(FlyingModel))]
    public abstract partial class UnitModel : ObjectModel
    {
        [field: Header("Unit Default Data")] 
        [MemoryPackInclude][field: SerializeField] public UnitType UnitType { get; protected set; }
        [MemoryPackInclude][field: SerializeField] protected float _moveSpeedDefault = 1f;
        [MemoryPackInclude][field: SerializeField] protected float _rotationSpeedDefault = 1f;
        [MemoryPackInclude][field: SerializeField] protected float _detectionRadiusDefault = 20f;
        [MemoryPackInclude][field: SerializeField] protected float _attackRangeDefault = 10f;
        [MemoryPackInclude][field: SerializeField] protected float _damageAmountDefault  = 10;
        [MemoryPackInclude][field: SerializeField] protected float _powerAttackDefault = 10f;
        
        [field: Header("Unit Changeable Data")]
        [MemoryPackInclude][field: SerializeField] public float MoveSpeedBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float RotationSpeedBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float DetectionRadiusBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float AttackRangeBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float DamageAmountBonus { get; set; } = 1;
        
        [field: Header("Secondary Data")]
        [MemoryPackInclude][field: SerializeField] public Vector3 DefaultAim { get; set; }
        
        public virtual float MoveSpeed => _moveSpeedDefault * MoveSpeedBonus;
        public virtual float RotationSpeed => _rotationSpeedDefault * RotationSpeedBonus;
        public virtual float DetectionRadius => _detectionRadiusDefault * DetectionRadiusBonus;
        public virtual float AttackRange => _attackRangeDefault * AttackRangeBonus;
        public virtual float DamageAmount => _damageAmountDefault * DamageAmountBonus;
        public virtual float PowerAttack => _powerAttackDefault;
    }
}