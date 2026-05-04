using System;
using MemoryPack;
using UnityEngine;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain
{
    [Serializable]
    [MemoryPackable]
    public partial class TerrainModel : ISavableModel
    {
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
        
        public virtual void LoadData(ISavableModel model)
        {
            if (model is not TerrainModel objectModel) return;
            SavePosition = objectModel.SavePosition;
            SaveRotation = objectModel.SaveRotation;
        }
        
        public ISavableModel GetSaveData()
        {
            return new TerrainModel
            {
                SavePosition = SavePosition,
                SaveRotation = SaveRotation,
            };
        }
    }
}