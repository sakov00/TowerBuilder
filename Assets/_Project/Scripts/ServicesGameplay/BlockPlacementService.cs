using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.SO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockPlacementService
    {
        [Inject] private AppData _appData;
        [Inject] private EffectPool _effectPool;
        [Inject] private BlockSpawnService _spawn;
        [Inject] private BuildingConfig _buildingConfig;
        [Inject] private GameplayFeedbackService _feedbackService;

        public void Resolve(BuildController current, BuildController previous)
        {
            float offset =
                current.transform.position.x -
                previous.transform.position.x;

            float absOffset = Mathf.Abs(offset);

            if (absOffset > _buildingConfig.PlacementTolerance)
            {
                Fail(current);
                return;
            }

            Place(current);

            if (absOffset <= _buildingConfig.PerfectPlacementTolerance)
            {
                HandlePerfectPlacement(current);
                return;
            }

            if (absOffset >= _buildingConfig.NearFailPlacementTolerance)
            {
                HandleNearMissPlacement();
                return;
            }

            HandleNormalPlacement();
        }

        private void Place(BuildController block)
        {
            _appData.LevelData.PlacedBlocksCount += 1;

            block.SetState(BuildState.Placed);
            block.SetKinematicState(RigidbodyType2D.Static);

            Debug.Log("Success");

            _spawn.SpawnNext().Forget();
        }

        private void Fail(BuildController block)
        {
            block.SetState(BuildState.Failed);
            block.DisposeDelayed().Forget();

            Debug.Log("Failed");

            _spawn.SpawnNext().Forget();
        }

        private void HandlePerfectPlacement(BuildController current)
        {
            _appData.LevelData.PerfectMultiplier += 1;
            _appData.LevelData.LevelScore += 5 * _appData.LevelData.PerfectMultiplier;

            _effectPool.Get(EffectType.Perfect, null, current.transform.position + new Vector3(0, _buildingConfig.BlockHeight / 2, 0));
            _feedbackService.ShowPerfect();
        }

        private void HandleNearMissPlacement()
        {
            _appData.LevelData.LevelScore += 1;
            _appData.LevelData.NearFailMultiplier += 1;

            _feedbackService.ShowNearMiss();
        }

        private void HandleNormalPlacement()
        {
            _appData.LevelData.LevelScore += 1;

            _appData.LevelData.PerfectMultiplier = 0;
            _appData.LevelData.NearFailMultiplier = 0;
        }
    }
}