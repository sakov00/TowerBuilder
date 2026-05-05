using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using UnityEngine;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSpawnService
    {
        private readonly BuildPool _pool;
        private readonly LiveRegistry _liveRegistry;

        public BlockSpawnService(BuildPool pool, LiveRegistry liveRegistry)
        {
            _pool = pool;
            _liveRegistry = liveRegistry;
        }

        public BuildController SpawnNext(Vector3 position)
        {
            var block = _pool.Get(BuildType.StartBlock, position);

            block.transform.position = position;

            block.SetState(BuildState.Swinging);
            block.SetKinematicState(RigidbodyType2D.Kinematic);

            _liveRegistry.Register(block);

            return block;
        }
    }
}