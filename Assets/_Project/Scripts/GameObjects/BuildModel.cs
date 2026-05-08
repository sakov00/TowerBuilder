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
        [Header("Object Default Data")]
        [MemoryPackInclude][SerializeField] private BuildType _buildType;
        [MemoryPackInclude][SerializeField] private BuildState _state = BuildState.Swinging;
        
        public BuildType BuildType
        {
            get => _buildType;
            set => _buildType = value;
        }

        public BuildState State
        {
            get => _state;
            set => _state = value;
        }
    }
}