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
        private Vector3 _startPosition;

        private float _currentAmplitude;
        private float _currentSpeed;

        [Inject]
        private void Construct()
        {
            _startPosition = _blocksContainer.transform.position;
            _currentAmplitude = _buildingConfig.SwayAmplitude.x;
            _currentSpeed = _buildingConfig.SwaySpeed;
        }

        public void Tick()
        {
            int blocksCount = _appData.LevelData.PlacedBlocksCount;

            if (blocksCount <= _buildingConfig.StartSwayFrom)
            {
                _blocksContainer.transform.position = _startPosition;
                return;
            }

            float t = Mathf.Clamp01((blocksCount - _buildingConfig.StartSwayFrom) / (float)_buildingConfig.MaxSwayFrom);
            float targetAmplitude = Mathf.Lerp(_buildingConfig.SwayAmplitude.x, _buildingConfig.SwayAmplitude.y, t);
            float targetSpeed = Mathf.Lerp(_buildingConfig.SwaySpeed, _buildingConfig.SwaySpeed * 2f, t);

            _currentAmplitude = Mathf.Lerp(_currentAmplitude, targetAmplitude, Time.deltaTime * 3f);
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * 3f);
            
            _time += Time.deltaTime * _currentSpeed;
            float offsetX = Mathf.Cos(_time) * _currentAmplitude;
            _blocksContainer.transform.position = _startPosition + new Vector3(offsetX, 0f, 0f);
        }
    }
}