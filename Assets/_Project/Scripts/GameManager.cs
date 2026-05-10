using System;
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
        [Inject] private SettingsService _settingsService;
        [Inject] private AppData _appData;
        [Inject] private SaveLoadLevelService _saveLoadLevelService;
        [Inject] private SceneCreator _sceneCreator;
        [Inject] private WindowsManager _windowsManager;
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private ApplicationEventsHandler _applicationEventsHandler;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private BlockSpawnService _blockSpawnService;
        [Inject] private CameraFollowService _cameraFollowService;
        [Inject] private BlocksContainer _blocksContainer;

        public virtual async UniTask RestartLevel()
        {
            _saveLoadLevelService.RemoveProgress(0);
            await StartLevel(0);
        }

        public virtual async UniTask StartLevel(int levelIndex)
        {
            Dispose();
            await _windowsManager.ShowWindow<LoadingWindow>();
            _windowsManager.HideFastWindow<MainMenuWindow>();
            Time.timeScale = 0;
            
            // await LoadLevel(levelIndex);
            
            _applicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused += OnApplicationPause;

            Time.timeScale = 1;
            
            await _blockSpawnService.SpawnStartBlock();
            await _blockSpawnService.SpawnNext();
            
            _windowsManager.ShowFastWindow<GameWindow>();
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
            _appData.LevelData.GameDisabled = true;
            foreach (var buildController in _liveRegistry.GetAllReactive().OfType<BuildController>())
            {
                buildController.SetKinematicState(RigidbodyType2D.Dynamic);
            }
            await UniTask.Delay(2000);
            await StartLevel(0);
            // await _windowsManager.ShowWindow<FailWindow>();
            // _windowsManager.HideFastWindow<GameWindow>();
        }

        public void Dispose()
        {
            _applicationEventsHandler.OnApplicationQuited -= OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused -= OnApplicationPause;
            _liveRegistry.GetAllReactive().ToList().ForEach(x => x.Dispose());
            _cameraFollowService.Reset();
            _appData.LevelData = new LevelData();
            _blocksContainer.Reset();
        }

        private void OnApplicationQuit()
        {
            // _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }

        private void OnApplicationPause(bool pause)
        {
            // if (pause)
            //     _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }
    }
}