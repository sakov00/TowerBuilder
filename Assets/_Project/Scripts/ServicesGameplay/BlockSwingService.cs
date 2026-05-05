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
        private float _range = 3f;
        private float _speed = 5f;

        public BlockSwingService(LiveRegistry liveRegistry)
        {
            _liveRegistry = liveRegistry;
        }

        public void Tick()
        {
            var blocks = _liveRegistry.GetAllReactive();

            // 🎯 берём текущий "качающийся" блок
            var block = blocks
                .OfType<BuildController>()
                .FirstOrDefault(b => b.Model.State == BuildState.Swinging);

            if (block == null)
                return;

            _time += Time.deltaTime * _speed;

            float x = Mathf.Sin(_time) * _range;

            var pos = block.transform.position;
            pos.x += x;

            block.transform.position = pos;
        }
    }
}