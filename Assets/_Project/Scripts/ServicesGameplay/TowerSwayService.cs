using System;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class TowerSwayService : ITickable
    {
        [Inject] private BlocksContainer _blocksContainer;
        [Inject] private BuildingConfig _buildingConfig;
        [Inject] private AppData _appData;

        private float _time;

        public void RegisterPlacementError(float offset)
        {
            _appData.LevelData.TotalSwayImbalance += offset;
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