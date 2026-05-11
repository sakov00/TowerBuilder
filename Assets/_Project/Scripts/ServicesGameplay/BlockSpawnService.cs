using System;
using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
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
using Random = UnityEngine.Random;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSpawnService
    {
        [Inject] private BuildPool _pool;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private BuildingConfig _buildingConfig;
        [Inject] private BlocksContainer _blocksContainer;
        [Inject] private CameraConfig _cameraConfig;
        [Inject] private AppData _appData;
        
        public async UniTask<BuildController> SpawnStartBlock()
        {
            var block = _pool.Get(BuildType.StartBlock, _blocksContainer.transform);
            block.transform.localScale = Vector3.one;
            
            var randomIndex = Random.Range(0, _buildingConfig.allBlockImages.Count);
            var randomSprite = _buildingConfig.allBlockImages[randomIndex];

            block.Initialize();
            block.SetImage(randomSprite);
            block.SetState(BuildState.Placed);
            block.SetKinematicState(RigidbodyType2D.Static);

            return block;
        }

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

            if (_appData.LevelData.HighestBlock != highest)
            {
                _appData.LevelData.HighestBlock = highest;
            }
            
            var block = _pool.Get(BuildType.StartBlock, null, 
                new Vector3(0, _appData.LevelData.HighestBlock.Transform.position.y + _cameraConfig.OffsetMoveY, 0));
            block.transform.localScale = Vector3.zero;
            
            var randomIndex = Random.Range(0, _buildingConfig.allBlockImages.Count);
            var randomSprite = _buildingConfig.allBlockImages[randomIndex];

            block.Initialize();
            block.SetImage(randomSprite);
            block.SetState(BuildState.Swinging);
            block.SetKinematicState(RigidbodyType2D.Static);
            
            await UniTask.Yield();
            await UniTask.Yield();
            await UniTask.Yield();
            
            block.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);

            return block;
        }
    }
}