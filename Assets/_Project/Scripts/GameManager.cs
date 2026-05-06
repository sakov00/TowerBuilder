using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
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
        
        private CompositeDisposable _disposables;
        
        public event Func<UniTaskVoid> WinEvent;
        public event Func<UniTaskVoid> FailEvent;

        public virtual async UniTask RestartLevel()
        {
            _saveLoadLevelService.RemoveProgress(_appData.User.CurrentLevel);
            await StartLevel(_appData.User.CurrentLevel);
        }

        public virtual async UniTask StartLevel(int levelIndex)
        {
            Dispose();
            await _windowsManager.ShowWindow<LoadingWindow>();
            _windowsManager.HideFastWindow<MainMenuWindow>();
            Time.timeScale = 0;
            
            // await LoadLevel(levelIndex);
            
            WinEvent += WinHandle;
            FailEvent += FailHandle;
            _applicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused += OnApplicationPause;

            Time.timeScale = 1;
            
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
        
        private async UniTaskVoid WinHandle()
        {
            await _windowsManager.ShowWindow<WinWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }

        private async UniTaskVoid FailHandle()
        {
            await _windowsManager.ShowWindow<FailWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
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
        
        public void Dispose()
        {
            _disposables?.Dispose();
            WinEvent -= WinHandle;
            FailEvent -= FailHandle;
            _applicationEventsHandler.OnApplicationQuited -= OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused -= OnApplicationPause;
            _disposables = new CompositeDisposable();
        }
    }
}