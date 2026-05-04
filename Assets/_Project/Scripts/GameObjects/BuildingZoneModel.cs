using System;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class BuildingZoneModel : ISavableModel
    {
        [MemoryPackInclude][field: SerializeField] public BuildType BuildType { get; private set; }
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        public virtual void LoadData(ISavableModel model)
        {
            if (model is BuildingZoneModel objectModel)
            {
                BuildType = objectModel.BuildType;
                SavePosition = objectModel.SavePosition;
                SaveRotation = objectModel.SaveRotation;
            }
        }
        
        public ISavableModel GetSaveData()
        {
            return new BuildingZoneModel
            {
                BuildType = BuildType,
                SavePosition = SavePosition,
                SaveRotation = SaveRotation
            };
        }
    }
}