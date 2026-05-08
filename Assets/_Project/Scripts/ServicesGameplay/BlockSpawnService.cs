using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSpawnService
    {
        [Inject] private BuildPool _pool;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private BuildingConfig _buildingConfig;
        [Inject] private CameraConfig _cameraConfig;
        
        private float _targetY;
        private BuildController _lastHighest;

        public async UniTask<BuildController> SpawnNext()
        {
            var blocks = _liveRegistry.GetAllReactive();

            var highest = blocks
                .OfType<BuildController>()
                .Where(b => b.Model.State == Enums.BuildState.Placed)
                .OrderBy(b => b.transform.position.y)
                .LastOrDefault();

            if (highest == null)
                return null;

            if (_lastHighest != highest)
            {
                _lastHighest = highest;
                _targetY = highest.Transform.position.y + _cameraConfig.OffsetMoveY;
            }
            
            var randomX = Random.Range(_buildingConfig.LimitMoveX.x, _buildingConfig.LimitMoveX.y);
            var block = _pool.Get(BuildType.StartBlock, null, new Vector3(randomX, _targetY + 3, 0));

            var randomIndex = Random.Range(0, _buildingConfig.allBlockImages.Count);
            var randomSprite = _buildingConfig.allBlockImages[randomIndex];

            block.SetImage(randomSprite);

            block.SetState(BuildState.Undefined);
            block.SetKinematicState(RigidbodyType2D.Static);

            block.transform.localScale = Vector3.zero;

            await block.transform
                .DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack);

            block.SetState(BuildState.Swinging);

            _liveRegistry.Register(block);

            return block;
        }
    }
}