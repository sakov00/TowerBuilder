using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.Build
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(MoneyBuildModel))]
    [MemoryPackUnion(1, typeof(FriendsBuildModel))]
    [MemoryPackUnion(2, typeof(TowerDefenceModel))]
    public abstract partial class BuildModel : ObjectModel
    {
        [field: Header("Object Default Data")] 
        [MemoryPackInclude][field: SerializeField] public BuildType BuildType { get; protected set; }

        [MemoryPackInclude][SerializeField] protected int _buildPriceDefault = 10;

        [field: Header("Object Changeable Data")]
        [MemoryPackInclude][field:SerializeField] public int BuildPriceBonus { get; set; } = 1;
        
        public virtual int BuildPrice => _buildPriceDefault * BuildPriceBonus;
    }
}