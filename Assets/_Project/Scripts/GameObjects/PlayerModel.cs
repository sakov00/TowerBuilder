using System;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class PlayerModel : ObjectModel
    {
        [field: Header("Unit Default Data")] 
        [MemoryPackInclude][field: SerializeField] protected float _moveSpeedDefault = 1f;
        [MemoryPackInclude][field: SerializeField] protected float _rotationSpeedDefault = 1f;
        [MemoryPackInclude][field: SerializeField] protected float _detectionRadiusDefault = 20f;
        [MemoryPackInclude][field: SerializeField] protected float _attackRangeDefault = 10f;
        [MemoryPackInclude][field: SerializeField] protected float _damageAmountDefault  = 10;
        [MemoryPackInclude][field: SerializeField] protected float _powerAttackDefault = 10f;
        
        [MemoryPackInclude][field: SerializeField] protected int _durationUltimateDefault = 5;
        [MemoryPackInclude][field: SerializeField] protected int _maxValueUltimateDefault = 100;
        [MemoryPackInclude] [field: SerializeField] protected int _shootAddUltimateDefault = 10;
        
        [MemoryPackInclude][field: SerializeField] protected int _durationTimeResurrectionDefault = 5;
        [MemoryPackInclude][field: SerializeField] protected int _secondsNoDamageDefault = 2;
        
        
        [field: Header("Unit Changeable Data")]
        [MemoryPackInclude][field: SerializeField] public float MoveSpeedBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float RotationSpeedBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float DetectionRadiusBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float AttackRangeBonus { get; set; } = 1f;
        [MemoryPackInclude][field: SerializeField] public float DamageAmountBonus { get; set; } = 1;
        
        [MemoryPackInclude][field: SerializeField] public int DurationUltimateBonus { get; set; } = 1;
        [MemoryPackInclude][field: SerializeField] public int MaxValueUltimateBonus { get; set; } = 1;
        [MemoryPackInclude] [field: SerializeField] public int ShootAddUltimateBonus { get; set; } = 1;
        
        [MemoryPackInclude][field: SerializeField] public int DurationTimeResurrectionBonus { get; set; } = 1;
        [MemoryPackInclude][field: SerializeField] public int SecondsNoDamageBonus { get; set; } = 1;
        
        [field: Header("Current Data")] 
        [MemoryPackInclude][field:SerializeField] public bool IsActiveUltimate { get; set; }
        [MemoryPackInclude][field:SerializeField] public int CurrentValueUltimate { get; set; }
        
        [MemoryPackInclude][field:SerializeField] public bool IsNoDamageable { get; set; }
        [MemoryPackInclude][field:SerializeField] public int CurrentTimeNoDamage { get; set; }
        [MemoryPackInclude][field:SerializeField] public int CurrentTimeResurrection { get; set; }
        
        [MemoryPackInclude][field:SerializeField] public bool IsKilled { get; set; }
        
        public virtual float MoveSpeed => _moveSpeedDefault * MoveSpeedBonus;
        public virtual float RotationSpeed => _rotationSpeedDefault * RotationSpeedBonus;
        public virtual float DetectionRadius => _detectionRadiusDefault * DetectionRadiusBonus;
        public virtual float AttackRange => _attackRangeDefault * AttackRangeBonus;
        public virtual float DamageAmount => _damageAmountDefault * DamageAmountBonus;
        public virtual float PowerAttack => _powerAttackDefault;
        
        public int DurationUltimate => _durationUltimateDefault * DurationUltimateBonus;
        public int MaxValueUltimate => _maxValueUltimateDefault * MaxValueUltimateBonus;
        public int ShootAddUltimate => _shootAddUltimateDefault * ShootAddUltimateBonus;
        
        public int DurationTimeResurrection => _durationTimeResurrectionDefault * DurationTimeResurrectionBonus;
        public int SecondsNoDamage => _secondsNoDamageDefault * SecondsNoDamageBonus;
    }
}