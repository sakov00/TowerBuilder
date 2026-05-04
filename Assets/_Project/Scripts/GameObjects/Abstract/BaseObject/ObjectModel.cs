using System;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;
using UnitModel = _Project.Scripts.GameObjects.Abstract.Unit.UnitModel;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(Build.BuildModel))]
    [MemoryPackUnion(1, typeof(MoneyBuildModel))]
    [MemoryPackUnion(2, typeof(FriendsBuildModel))]
    [MemoryPackUnion(3, typeof(TowerDefenceModel))]
    [MemoryPackUnion(4, typeof(UnitModel))]
    [MemoryPackUnion(5, typeof(PlayerModel))]
    [MemoryPackUnion(6, typeof(WarriorModel))]
    [MemoryPackUnion(7, typeof(ArcherModel))]
    [MemoryPackUnion(8, typeof(FlyingModel))]
    public abstract partial class ObjectModel : ISavableModel
    {
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        [field: Header("Object Default Data")] 
        [MemoryPackInclude][field:SerializeField] public WarSide WarSide { get; protected set; }
        [MemoryPackInclude][field:SerializeField] protected float _maxHealthDefault = 100f;
        [MemoryPackInclude][field:SerializeField] protected float _delayRegenerationDefault = 3f;
        [MemoryPackInclude][field:SerializeField] protected float _regenerateHealthInSecondDefault = 5f;
        
        [field: Header("Object Changeable Data")]  
        [MemoryPackInclude][field:SerializeField] protected float MaxHealthBonus { get; set; } = 1f;
        [MemoryPackInclude][field:SerializeField] protected float DelayRegenerationBonus { get; set; } = 1f;
        [MemoryPackInclude][field:SerializeField] protected float RegenerateHealthInSecondBonus { get; set; } = 1f;
        [MemoryPackInclude][field:SerializeField] protected float _currentHealth = 100f;

        public virtual float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }

        public virtual float MaxHealth => _maxHealthDefault * MaxHealthBonus;
        public virtual float DelayRegeneration => _delayRegenerationDefault * DelayRegenerationBonus;
        public virtual float RegenerateHealthInSecond => _regenerateHealthInSecondDefault * RegenerateHealthInSecondBonus;
    }
}