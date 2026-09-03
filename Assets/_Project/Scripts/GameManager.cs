using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using _Project.Scripts.ServicesGameplay;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts
{
    public class GameManager : IDisposable
    {
        private AppData _appData;
        private SaveLoadLevelService _saveLoadLevelService;
        private SceneCreator _sceneCreator;
        private WindowsManager _windowsManager;
        private SaveRegistry _saveRegistry;
        private ApplicationEventsHandler _applicationEventsHandler;
        private LiveRegistry _liveRegistry;
        private BlockSpawnService _blockSpawnService;
        private CameraFollowService _cameraFollowService;
        private BlocksContainer _blocksContainer;
        private AnalyticService _analyticService;
        private AdsService _adsService;
        private EnvironmentMovementService _environmentMovementService;
        private MultiplyZonesRegistry _multiplyZonesRegistry;
        
        [Inject]
        public GameManager(AppData appData, SaveLoadLevelService saveLoadLevelService, SceneCreator sceneCreator,
            WindowsManager windowsManager, SaveRegistry saveRegistry, ApplicationEventsHandler applicationEventsHandler, 
            LiveRegistry liveRegistry, BlocksContainer blocksContainer, BlockSpawnService blockSpawnService, 
            CameraFollowService cameraFollowService, AnalyticService analyticService, AdsService adsService,
            EnvironmentMovementService environmentMovementService, MultiplyZonesRegistry multiplyZonesRegistry)
        {
            _appData = appData;
            _saveLoadLevelService = saveLoadLevelService;
            _sceneCreator = sceneCreator;
            _windowsManager = windowsManager;
            _saveRegistry = saveRegistry;
            _applicationEventsHandler = applicationEventsHandler;
            _liveRegistry = liveRegistry;
            _blockSpawnService = blockSpawnService;
            _cameraFollowService = cameraFollowService;
            _blocksContainer = blocksContainer;
            _analyticService = analyticService;
            _adsService = adsService;
            _environmentMovementService = environmentMovementService;
            _multiplyZonesRegistry = multiplyZonesRegistry;
        }

        public virtual async UniTask RestartLevel()
        {
            _saveLoadLevelService.RemoveProgress(0);
            await StartLevel(0);
        }

        public virtual async UniTask StartLevel(int levelIndex, bool useLoadingScreen = true)
        {
            if (useLoadingScreen)
                await _windowsManager.ShowWindow<LoadingWindow>();
            
            Time.timeScale = 0;
            
            Dispose();
            
            // await LoadLevel(levelIndex);
            
            _applicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused += OnApplicationPause;

            Time.timeScale = 1;
            _multiplyZonesRegistry.GetAll().ForEach(x => x.Reset());
            _environmentMovementService.Restart();
            await _blockSpawnService.SpawnStartBlock();
            await _blockSpawnService.SpawnNext();
            _appData.LevelData.GameDisabled = false;
            
            _windowsManager.ShowFastWindow<GameWindow>();
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            gameWindow.Reset();
            
            if (useLoadingScreen)
                await _windowsManager.HideWindow<LoadingWindow>();
        }
        
        public virtual async UniTask LoadLevel(int levelIndex, bool isInitialize = true)
        {
            // foreach (var obj in _saveRegistry.GetAllByType<IPoolableDispose>())
            //     obj.Dispose();
            //
            _liveRegistry.Clear();
            _saveRegistry.Clear();
            
            await _saveLoadLevelService.LoadLevel(levelIndex);
            await _sceneCreator.InstantiateObjects(_appData.LevelData.SavableModels, isInitialize);
        }
        
        public async UniTaskVoid WinHandle()
        {
            await _windowsManager.ShowWindow<WinWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }

        public async UniTaskVoid FailHandle()
        {
            _analyticService.SendMessage("Fail");
            _appData.User.ScoreRecord = _appData.LevelData.LevelScore;
            _appData.LevelData.GameDisabled = true;
            foreach (var buildController in _liveRegistry.GetAllReactive().OfType<BuildController>())
            {
                buildController.SetKinematicState(RigidbodyType2D.Dynamic);
            }
            await UniTask.Delay(2000);
            _adsService.UseInter(() => StartLevel(0).Forget());
            // await _windowsManager.ShowWindow<FailWindow>();
            // _windowsManager.HideFastWindow<GameWindow>();
        }

        public void Dispose()
        {
            _appData.User.ScoreRecord = _appData.LevelData.LevelScore;
            _applicationEventsHandler.OnApplicationQuited -= OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused -= OnApplicationPause;
            _liveRegistry.GetAllReactive().ToList().ForEach(x => x.Dispose());
            _cameraFollowService.Reset();
            _appData.LevelData.SetData(new LevelData());
            _blocksContainer.Reset();
            
        }

        private void OnApplicationQuit()
        {
            _appData.User.ScoreRecord = _appData.LevelData.LevelScore;
            // _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                _appData.User.ScoreRecord = _appData.LevelData.LevelScore;
                // _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }
    }
}