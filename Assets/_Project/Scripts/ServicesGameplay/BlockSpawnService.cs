using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSpawnService
    {
        [Inject] private BuildPool _pool;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private BuildingConfig _buildingConfig;

        public BuildController SpawnNext()
        {
            var cameraPos = GlobalObjects.Camera.transform.position;
            var block = _pool.Get(BuildType.StartBlock, null, new Vector3(0, cameraPos.y + 2, 0));
            
            var randomIndex = Random.Range(0, _buildingConfig.allBlockImages.Count);
            var randomSprite = _buildingConfig.allBlockImages[randomIndex];
            
            block.SetImage(randomSprite);

            block.SetState(BuildState.Swinging);
            block.SetKinematicState(RigidbodyType2D.Static);

            _liveRegistry.Register(block);

            return block;
        }
    }
}