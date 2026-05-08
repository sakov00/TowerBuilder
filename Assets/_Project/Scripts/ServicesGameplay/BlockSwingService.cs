using System.Linq;
using UnityEngine;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSwingService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private BuildingConfig _buildingConfig;

        private float _time;
        private BuildController _current;
        private float _direction;

        public void Tick()
        {
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
                _current = block;
                float offset = block.transform.position.x;
                _direction = Random.value < 0.5f ? -1 : 1;
                _time = Mathf.Asin(
                    Mathf.Clamp(offset / _buildingConfig.SwingRange, -1f, 1f)
                );
            }
            
            _time += Time.deltaTime * _buildingConfig.SwingSpeed * _direction;
            float offsetX = Mathf.Sin(_time) * _buildingConfig.SwingRange;
            float x = offsetX;

            x = Mathf.Clamp(
                x,
                _buildingConfig.LimitMoveX.x,
                _buildingConfig.LimitMoveX.y
            );

            var pos = block.transform.position;
            pos.x = x;
            block.transform.position = pos;
        }
    }
}