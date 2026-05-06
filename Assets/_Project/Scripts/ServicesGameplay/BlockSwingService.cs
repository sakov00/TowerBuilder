using System.Linq;
using UnityEngine;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSwingService : ITickable
    {
        private readonly LiveRegistry _liveRegistry;

        private float _time;
        private float _range = 1;
        private float _speed = 5f;

        private BuildController _current;
        private float _startX;

        public BlockSwingService(LiveRegistry liveRegistry)
        {
            _liveRegistry = liveRegistry;
        }

        public void Tick()
        {
            var blocks = _liveRegistry.GetAllReactive();

            var block = blocks
                .OfType<BuildController>()
                .FirstOrDefault(b => b.Model.State == BuildState.Swinging);

            if (block == null)
                return;

            if (_current != block)
            {
                _current = block;
                _startX = block.transform.position.x;
                _time = 0f;
            }

            _time += Time.deltaTime * _speed;

            float offset = Mathf.Sin(_time) * _range;

            var pos = block.transform.position;
            pos.x = _startX + offset;

            block.transform.position = pos;
        }
    }
}