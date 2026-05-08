using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using MemoryPack;
using UniRx;

namespace _Project.Scripts.AllAppData
{
    [MemoryPackable]
    public partial class LevelData
    {
        public List<ISavableModel> SavableModels { get; set; } = new();
        
        [MemoryPackIgnore] public readonly IntReactiveProperty PlacedBlocksCountReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty LevelScoreReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty PerfectMultiplierReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty NearFailMultiplierReactive;
        
        public int PlacedBlocksCount
        {
            get => PlacedBlocksCountReactive.Value;
            set => PlacedBlocksCountReactive.Value = value;
        }

        public int LevelScore
        {
            get => LevelScoreReactive.Value;
            set => LevelScoreReactive.Value = value;
        }
        
        public int PerfectMultiplier
        {
            get => PerfectMultiplierReactive.Value;
            set => PerfectMultiplierReactive.Value = value;
        }
        
        public int NearFailMultiplier
        {
            get => NearFailMultiplierReactive.Value;
            set => NearFailMultiplierReactive.Value = value;
        }

        public LevelData()
        {
            PlacedBlocksCountReactive = new IntReactiveProperty(0);
            LevelScoreReactive = new IntReactiveProperty(0);
            PerfectMultiplierReactive = new IntReactiveProperty(0);
            NearFailMultiplierReactive = new IntReactiveProperty(0);
        }

        public void SetData(LevelData levelData)
        {
            SavableModels = levelData.SavableModels;
            PlacedBlocksCountReactive.Value = levelData.PlacedBlocksCount;
            LevelScoreReactive.Value = levelData.LevelScore;
            PerfectMultiplierReactive.Value = levelData.PerfectMultiplier;
            NearFailMultiplierReactive.Value = levelData.NearFailMultiplier;
        }
    }
}