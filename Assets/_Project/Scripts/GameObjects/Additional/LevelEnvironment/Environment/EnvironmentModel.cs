using System;
using _Project.Scripts.Enums;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment
{
    [Serializable]
    [MemoryPackable]
    public partial class EnvironmentModel : ISavableModel
    {
        [MemoryPackInclude][field: SerializeField] public EnvironmentType EnvironmentType { get; set; }
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        public virtual void LoadData(ISavableModel model)
        {
            if (model is not EnvironmentModel objectModel) return;
            EnvironmentType = objectModel.EnvironmentType;
            SavePosition = objectModel.SavePosition;
            SaveRotation = objectModel.SaveRotation;
        }
        
        public ISavableModel GetSaveData()
        {
            return new EnvironmentModel
            {
                EnvironmentType = EnvironmentType,
                SavePosition = SavePosition,
                SaveRotation = SaveRotation
            };
        }
    }
}