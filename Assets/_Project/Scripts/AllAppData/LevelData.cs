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
        [Inject] private PlayerConfig _playerConfig;
        public List<ISavableModel> SavableModels { get; set; } = new();

        [MemoryPackIgnore] public readonly BoolReactiveProperty GameDisabledReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty HealthReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty PlacedBlocksCountReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty LevelScoreReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty CurrentSwayAmplitudeReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty CurrentSwaySpeedReactive;
        [MemoryPackIgnore] public readonly FloatReactiveProperty TotalSwayImbalanceReactive;
        
        public bool GameDisabled
        {
            get => GameDisabledReactive.Value;
            set => GameDisabledReactive.Value = value;
        }
        
        public int Health
        {
            get => HealthReactive.Value;
            set => HealthReactive.Value = value;
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

        public int AddScoreValue { get; set; } = 1;
        
        public int PerfectComboValue { get;set; }
        public int PerfectMultiplier { get; set; } = 5;
        
        public int NearFailComboValue { get;set; }
        public int NearFailMultiplier { get; set; } = 1;
        
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
            HealthReactive = new IntReactiveProperty(_playerConfig.MaxHealth);
            PlacedBlocksCountReactive = new IntReactiveProperty(1);
            
            LevelScoreReactive = new IntReactiveProperty(0);
            AddScoreValue = 1;
            PerfectComboValue = 0;
            PerfectMultiplier = 5;
            NearFailComboValue = 0;
            NearFailMultiplier = 1;
        
            CurrentSwayAmplitudeReactive = new FloatReactiveProperty(_buildingConfig.SwayAmplitude.x);
            CurrentSwaySpeedReactive = new FloatReactiveProperty(_buildingConfig.SwaySpeed);
            TotalSwayImbalanceReactive = new FloatReactiveProperty(0);
            HighestBlock = null;
        }

        public void SetData(LevelData levelData)
        {
            SavableModels = levelData.SavableModels;
            HealthReactive.Value = levelData.Health;
            PlacedBlocksCountReactive.Value = levelData.PlacedBlocksCount;
            
            LevelScoreReactive.Value = levelData.LevelScore;
            AddScoreValue = levelData.AddScoreValue;
            PerfectComboValue = levelData.PerfectComboValue;
            PerfectMultiplier = levelData.PerfectMultiplier;
            NearFailComboValue = levelData.NearFailComboValue;
            NearFailMultiplier = levelData.NearFailMultiplier;
            
            CurrentSwayAmplitudeReactive.Value = levelData.CurrentSwayAmplitude;
            CurrentSwaySpeedReactive.Value = levelData.CurrentSwaySpeed;
            TotalSwayImbalanceReactive.Value = levelData.TotalSwayImbalance;
            HighestBlock = null;
        }
    }
}