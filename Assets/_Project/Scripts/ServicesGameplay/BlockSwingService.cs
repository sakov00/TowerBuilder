using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using UnityEngine;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSwingService : ITickable
    {
        private readonly LiveRegistry _liveRegistry;
        private readonly BuildingConfig _buildingConfig;
        private readonly AppData _appData;

        private BuildController _current;

        private float _time;
        private float _direction;
        private Vector3 _center;

        [Inject]
        public BlockSwingService(LiveRegistry liveRegistry, BuildingConfig buildingConfig, AppData appData)
        {
            _liveRegistry = liveRegistry;
            _buildingConfig = buildingConfig;
            _appData = appData;
        }

        public void Tick()
        {
            if(_appData.LevelData.GameDisabled)
                return;
            
            var block = _liveRegistry.GetAllReactive()
                .OfType<BuildController>()
                .FirstOrDefault(b => b.Model.State == BuildState.Swinging);

            if (block == null)
            {
                _current = null;
                return;
            }

            if (_current != block)
            {
                InitBlock(block);
            }

            Move(block);
        }

        private void InitBlock(BuildController block)
        {
            _current = block;
            _direction = Random.value < 0.5f ? -1f : 1f;
            _center = block.transform.position + Vector3.up * _buildingConfig.SwingHeight;
            _time = _direction > 0 ? -Mathf.PI * 0.5f : Mathf.PI * 0.5f;
        }

        private void Move(BuildController block)
        {
            _time += Time.deltaTime * _buildingConfig.SwingSpeed * _direction;
            _time = Mathf.Clamp(_time, -Mathf.PI * 0.5f, Mathf.PI * 0.5f);
            
            float x = Mathf.Sin(_time) * _buildingConfig.SwingRange.x;
            float y = -Mathf.Cos(_time) * _buildingConfig.SwingRange.y;

            Vector3 pos = _center + new Vector3(x, y, 0f);

            block.transform.position = pos;

            if (_time >= Mathf.PI * 0.5f)
            {
                _direction = -1f;
            }
            else if (_time <= -Mathf.PI * 0.5f)
            {
                _direction = 1f;
            }

            Rotate(block);
        }

        private void Rotate(BuildController block)
        {
            float tilt =
                Mathf.Sin(_time) *
                _buildingConfig.SwingTilt;

            block.transform.rotation =
                Quaternion.Euler(0f, 0f, -tilt);
        }
    }
}