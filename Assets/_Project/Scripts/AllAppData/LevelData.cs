using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using MemoryPack;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.AllAppData
{
    [MemoryPackable]
    public partial class LevelData
    {
        public List<ISavableModel> SavableModels { get; set; } = new();
        public List<ISavableModel> ObjectsForRestoring { get; set; } = new();

        public void SetData(LevelData levelData)
        {
            SavableModels = levelData.SavableModels;
            ObjectsForRestoring = levelData.ObjectsForRestoring;
        }
    }
}