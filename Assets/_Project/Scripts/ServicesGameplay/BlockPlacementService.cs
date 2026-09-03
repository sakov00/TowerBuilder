using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.SO;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockPlacementService
    {
        private readonly AppData _appData;
        private readonly EffectPool _effectPool;
        private readonly SettingsService _settingsService;
        private readonly BlockSpawnService _spawn;
        private readonly TowerSwayService _towerSway;
        private readonly BuildingConfig _buildingConfig;
        private readonly BlocksContainer _blocksContainer;
        private readonly GameplayFeedbackService _feedbackService;
        private readonly LiveRegistry _liveRegistry;
        private readonly GameManager _gameManager;
        private readonly WindowsManager _windowsManager;
        private MultiplyZonesRegistry _multiplyZonesRegistry;

        [Inject]
        public BlockPlacementService(AppData appData, EffectPool effectPool, SettingsService settingsService,
            BlockSpawnService spawn, TowerSwayService towerSway, BuildingConfig buildingConfig,
            BlocksContainer blocksContainer, GameplayFeedbackService feedbackService,
            LiveRegistry liveRegistry, GameManager gameManager, WindowsManager windowsManager, MultiplyZonesRegistry multiplyZonesRegistry)
        {
            _appData = appData;
            _effectPool = effectPool;
            _settingsService = settingsService;
            _spawn = spawn;
            _towerSway = towerSway;
            _buildingConfig = buildingConfig;
            _blocksContainer = blocksContainer;
            _feedbackService = feedbackService;
            _liveRegistry = liveRegistry;
            _gameManager = gameManager;
            _windowsManager = windowsManager;
            _multiplyZonesRegistry = multiplyZonesRegistry;
        }

        public async UniTask Resolve(BuildController current)
        {
            var blocks = _liveRegistry.GetAllReactive();

            var highest = blocks
                .OfType<BuildController>()
                .Where(b => b.Model.State == Enums.BuildState.Placed)
                .OrderBy(b => b.transform.position.y)
                .LastOrDefault();
            
            if (highest == null)
                return;
            
            float offset =
                current.transform.position.x -
                highest.transform.position.x;

            float absOffset = Mathf.Abs(offset);

            if (absOffset > _buildingConfig.PlacementTolerance)
            {
                Fail(current);
                return;
            }
            
            Place(current, highest);

            if (absOffset <= _buildingConfig.PerfectPlacementTolerance)
            {
                HandlePerfectPlacement(current, highest);
                return;
            }

            if (absOffset >= _buildingConfig.NearFailPlacementTolerance)
            {
                HandleNearMissPlacement();
                return;
            }

            HandleNormalPlacement();
        }

        private void Place(BuildController current, BuildController highest)
        {
            _appData.LevelData.PlacedBlocksCount += 1;
            
            current.transform.SetParent(_blocksContainer.transform);
            current.SetState(BuildState.Placed);
            current.SetKinematicState(RigidbodyType2D.Static);
            
            var targetLocalPos = new Vector3(
                current.transform.localPosition.x,
                Mathf.Round(highest.transform.localPosition.y + _buildingConfig.BlockHeight),
                current.transform.localPosition.z);
            
            _towerSway.RegisterPlacementError(current.transform.localPosition.x);
            current.transform.DOLocalMove(targetLocalPos, 0.25f);
            current.transform.DOLocalRotate(Vector3.zero, 0.25f);
            
            _settingsService.PlaySfx(SoundKey.BlockPlaced);
            Debug.Log("Success");
            
            if (_appData.LevelData.PlacedBlocksCount > _buildingConfig.StartSwayFrom)
            {
                _windowsManager.GetWindow<GameWindow>().ShowBalancePanel();
            }
            
            if (Mathf.Abs(_appData.LevelData.TotalSwayImbalance) > _buildingConfig.DestroyImbalance)
            {
                _gameManager.FailHandle().Forget();
                return;
            }
            
            if(_appData.LevelData.PlacedBlocksCount >= _buildingConfig.TutorialBlocksCount)
                _appData.User.IsTutorialBlocksPassed = true;
            else
                _appData.User.IsTutorialFirstBlockPassed = true;
            
            _multiplyZonesRegistry.GetAll().ForEach(x => x.IsCompletedStage(current));
            _spawn.SpawnNext().Forget();
        }

        private void Fail(BuildController block)
        {
            block.SetState(BuildState.Failed);
            block.DisposeDelayed().Forget();
            
            GlobalObjects.Camera.DOShakePosition(0.2f, 0.3f, 20, 90f);
            _settingsService.PlayVibrationPop();
            _settingsService.PlaySfx(SoundKey.BlockFailed);
            Debug.Log("Failed");

            if (_appData.User.IsTutorialBlocksPassed)
            {
                _appData.LevelData.Health--;
                if (_appData.LevelData.Health <= 0)
                {
                    _gameManager.FailHandle().Forget();
                    return;
                }
            }

            _spawn.SpawnNext().Forget();
        }

        private void HandlePerfectPlacement(BuildController current, BuildController highest)
        {
            _appData.LevelData.PerfectComboValue += 1;
            _appData.LevelData.LevelScore += _appData.LevelData.AddScoreValue *
                                             _appData.LevelData.PerfectMultiplier * _appData.LevelData.PerfectComboValue;

            var targetLocalPos = new Vector3(
                highest.transform.localPosition.x,
                Mathf.Round(highest.transform.localPosition.y + _buildingConfig.BlockHeight),
                highest.transform.localPosition.z);

            current.transform.DOLocalMove(targetLocalPos, 0.25f);
            _effectPool.Get(EffectType.Perfect, current.transform, 
                current.transform.position + new Vector3(0, _buildingConfig.BlockHeight / 2, 0));

            _feedbackService.ShowPerfect();
        }

        private void HandleNearMissPlacement()
        {
            _appData.LevelData.LevelScore += _appData.LevelData.AddScoreValue;
            _appData.LevelData.NearFailComboValue += 1;

            _feedbackService.ShowNearMiss();
        }

        private void HandleNormalPlacement()
        {
            _appData.LevelData.LevelScore += _appData.LevelData.AddScoreValue;

            _appData.LevelData.PerfectComboValue = 0;
            _appData.LevelData.NearFailComboValue = 0;
        }
    }
}