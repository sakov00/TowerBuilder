using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Build;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
using _Project.Scripts.Interfaces;
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

        public virtual async UniTask ResetRound()
        {
            await _sceneCreator.InstantiateObjects(_appData.LevelData.ObjectsForRestoring);
            _windowsManager.ShowFastWindow<GameWindow>();
        }

        public virtual async UniTask RestartLevel()
        {
            _saveLoadLevelService.RemoveProgress(_appData.User.CurrentLevel);
            await StartLevel(_appData.User.CurrentLevel);
        }

        public virtual async UniTask StartLevel(int levelIndex)
        {
            Dispose();
            
            Time.timeScale = 0;
            
            await LoadLevel(levelIndex);
            
            WinEvent += WinHandle;
            FailEvent += FailHandle;
            _applicationEventsHandler.OnApplicationQuited += OnApplicationQuit;
            _applicationEventsHandler.OnApplicationPaused += OnApplicationPause;

            Time.timeScale = 1;
        }
        
        public virtual async UniTask LoadLevel(int levelIndex, bool isInitialize = true)
        {
            foreach (var obj in _saveRegistry.GetAllByType<IPoolableDispose>())
                obj.Dispose();
            
            _liveRegistry.Clear();
            _saveRegistry.Clear();
            
            await _saveLoadLevelService.LoadLevel(levelIndex);
            await _sceneCreator.InstantiateObjects(_appData.LevelData.SavableModels, isInitialize);
        }
        
        private void NextRoundOnClick()
        {
            _appData.LevelData.IsFighting = true;
            _appData.LevelData.ObjectsForRestoring = _saveRegistry.GetAll()
                .Select(o => o.GetSavableModel()).ToList();
            
            _saveRegistry.GetAllByType<EnemyRoadController>().ForEach(x => x.StartSpawn());
            
            _liveRegistry.OnRemoveAsObservable()
                .Subscribe(removedObject => TryInvokeAllEnemiesKilled(removedObject.Value)).AddTo(_disposables);
            
            _liveRegistry.OnRemoveAsObservable()
                .Subscribe(removedObject => TryInvokeMainBuildDestroyed(removedObject.Value)).AddTo(_disposables);
        }
        
        private void TryInvokeAllEnemiesKilled(ObjectController objectController)
        {
            // if (objectController.WarSide == WarSide.Enemy &&
            //     _liveRegistry.GetAllByType<UnitController>().All(x => x.WarSide != WarSide.Enemy))
            //     WinEvent?.Invoke();
        }
        
        private void TryInvokeMainBuildDestroyed(ObjectController objectController)
        {
            if (objectController is MainBuildController)
                FailEvent?.Invoke();
        }
        
        private async UniTaskVoid WinHandle()
        {
            _appData.LevelData.IsFighting = false;
            await _windowsManager.ShowWindow<WinWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }

        private async UniTaskVoid FailHandle()
        {
            _appData.LevelData.IsFighting = false;
            await _windowsManager.ShowWindow<FailWindow>();
            _windowsManager.HideFastWindow<GameWindow>();
        }
        
        private void OnApplicationQuit()
        {
            _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
        }
        
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                _saveLoadLevelService?.SaveLevelProgress(_appData.User.CurrentLevel).GetAwaiter().GetResult();
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