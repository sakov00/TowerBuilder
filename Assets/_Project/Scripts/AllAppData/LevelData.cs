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

        [MemoryPackIgnore] public readonly IntReactiveProperty CurrentRoundReactive;
        [MemoryPackIgnore] public readonly IntReactiveProperty LevelMoneyReactive;
        [MemoryPackIgnore] public readonly BoolReactiveProperty IsFightingReactive;
        [MemoryPackIgnore] public readonly BoolReactiveProperty IsStrategyModeReactive;
        
        public int CurrentRound
        {
            get => CurrentRoundReactive.Value;
            set => CurrentRoundReactive.Value = value;
        }

        public int LevelMoney
        {
            get => LevelMoneyReactive.Value;
            set => LevelMoneyReactive.Value = value;
        }

        public bool IsFighting
        {
            get => IsFightingReactive.Value;
            set => IsFightingReactive.Value = value;
        }
        
        public bool IsStrategyMode
        {
            get => IsStrategyModeReactive.Value;
            set => IsStrategyModeReactive.Value = value;
        }
        
        [MemoryPackIgnore] public Vector3 MoveDirection { get; set; }

        public LevelData()
        {
            CurrentRoundReactive = new IntReactiveProperty(0);
            LevelMoneyReactive = new IntReactiveProperty(0);
            IsFightingReactive = new BoolReactiveProperty(false);
            IsStrategyModeReactive = new BoolReactiveProperty(false);
        }

        public void SetData(LevelData levelData)
        {
            SavableModels = levelData.SavableModels;
            ObjectsForRestoring = levelData.ObjectsForRestoring;
            CurrentRoundReactive.Value = levelData.CurrentRound;
            LevelMoneyReactive.Value = levelData.LevelMoney;
            IsFightingReactive.Value = levelData.IsFighting;
            IsStrategyModeReactive.Value = levelData.IsStrategyMode;
        }
    }
}