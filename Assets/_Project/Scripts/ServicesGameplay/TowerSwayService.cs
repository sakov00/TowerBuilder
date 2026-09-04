using System;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects;
using _Project.Scripts.SO;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class TowerSwayService : ITickable
    {
        private readonly BlocksContainer _blocksContainer;
        private readonly BuildingConfig _buildingConfig;
        private readonly AppData _appData;
        private readonly WindowsManager _windowsManager;

        private float _time;

        [Inject]
        public TowerSwayService(BlocksContainer blocksContainer, BuildingConfig buildingConfig, AppData appData, WindowsManager windowsManager)
        {
            _blocksContainer = blocksContainer;
            _buildingConfig = buildingConfig;
            _appData = appData;
            _windowsManager = windowsManager;
        }

        public void RegisterPlacementError(float offset)
        {
            _appData.LevelData.TotalSwayImbalance += offset;
            
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            if (Mathf.Abs(_appData.LevelData.TotalSwayImbalance) > _buildingConfig.DestroyImbalanceRed)
            {
                if(!_appData.User.IsTutorialBoosterBalancePassed)
                    gameWindow.ShowTutorialBoosterBalanceReward();
                gameWindow.SetBackLightBalance(true, Color.red).Forget();
            }
            else if (Mathf.Abs(_appData.LevelData.TotalSwayImbalance) > _buildingConfig.DestroyImbalanceYellow)
            {
                if(!_appData.User.IsTutorialBalancePassed)
                    gameWindow.ShowTutorialBalance();
                gameWindow.SetBackLightBalance(true, Color.yellow).Forget();
            }
            else
                gameWindow.SetBackLightBalance(false, Color.clear).Forget();
        }

        public void Tick()
        {
            if(_appData.LevelData.GameDisabled)
                return;
            
            int blocksCount = _appData.LevelData.PlacedBlocksCount;

            float t = Mathf.Clamp01((blocksCount - _buildingConfig.StartSwayFrom) / (float)_buildingConfig.MaxSwayFrom);
            float baseAmplitude = Mathf.Lerp(_buildingConfig.SwayAmplitude.y, _buildingConfig.SwayAmplitude.x, t);
            float targetAmplitude = baseAmplitude;
            float targetSpeed = _buildingConfig.SwaySpeed + Mathf.Abs(_appData.LevelData.TotalSwayImbalance) * _buildingConfig.SwaySensitivityImbalance;

            _appData.LevelData.CurrentSwayAmplitude = Mathf.Lerp(_appData.LevelData.CurrentSwayAmplitude, targetAmplitude, Time.deltaTime);
            _appData.LevelData.CurrentSwaySpeed = Mathf.Lerp(_appData.LevelData.CurrentSwaySpeed, targetSpeed, Time.deltaTime);
    
            _time += Time.deltaTime * _appData.LevelData.CurrentSwaySpeed;
            float angleZ = Mathf.Cos(_time) * _appData.LevelData.CurrentSwayAmplitude;
            _blocksContainer.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }
}