using System;
using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.SO;
using MemoryPack;
using UniRx;
using VContainer;

namespace _Project.Scripts.AllAppData
{
    [MemoryPackable]
    public partial class LevelData
    {
        [Inject] private BuildingConfig _buildingConfig;
        public List<ISavableModel> SavableModels { get; set; } = new();

        [MemoryPackIgnore] public readonly BoolReactiveProperty GameDisabledReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty PlacedBlocksCountReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty LevelScoreReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty PerfectMultiplierReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty NearFailMultiplierReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty CurrentSwayAmplitudeReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty CurrentSwaySpeedReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty TotalSwayImbalanceReactive;
        
        public bool GameDisabled
        {
            get => GameDisabledReactive.Value;
            set => GameDisabledReactive.Value = value;
        }
        
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
        
        public float CurrentSwayAmplitude
        {
            get => CurrentSwayAmplitudeReactive.Value;
            set => CurrentSwayAmplitudeReactive.Value = value;
        }
        
        public float CurrentSwaySpeed
        {
            get => CurrentSwaySpeedReactive.Value;
            set => CurrentSwaySpeedReactive.Value = value;
        }
        
        public float TotalSwayImbalance
        {
            get => TotalSwayImbalanceReactive.Value;
            set => TotalSwayImbalanceReactive.Value = value;
        }
        
        [MemoryPackIgnore] public BuildController HighestBlock { get; set; }

        public LevelData()
        {
            InjectManager.Inject(this);
            GameDisabledReactive = new BoolReactiveProperty(false);
            PlacedBlocksCountReactive = new IntReactiveProperty(0);
            LevelScoreReactive = new IntReactiveProperty(0);
            PerfectMultiplierReactive = new IntReactiveProperty(0);
            NearFailMultiplierReactive = new IntReactiveProperty(0);
            CurrentSwayAmplitudeReactive = new FloatReactiveProperty(_buildingConfig.SwayAmplitude.x);
            CurrentSwaySpeedReactive = new FloatReactiveProperty(_buildingConfig.SwaySpeed);
            TotalSwayImbalanceReactive = new FloatReactiveProperty(0);
            HighestBlock = null;
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