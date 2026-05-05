using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class BuildModel : ObjectModel
    {
        [field: Header("Object Default Data")] 
        [MemoryPackInclude][field: SerializeField] public BuildType BuildType { get; protected set; }
        [MemoryPackInclude][field: SerializeField] public BuildState State { get; set; } = BuildState.Swinging;
        [MemoryPackInclude][SerializeField] protected int _buildPriceDefault = 10;

        [field: Header("Object Changeable Data")]
        [MemoryPackInclude][field:SerializeField] public int BuildPriceBonus { get; set; } = 1;
        
        public virtual int BuildPrice => _buildPriceDefault * BuildPriceBonus;
    }
}