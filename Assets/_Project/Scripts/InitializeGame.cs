using System;
using System.Linq;
using System.Threading;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace _Project.Scripts
{
    public class InitializeGame : IInitializable, IAsyncStartable
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private GameManager _gameManager;
        [Inject] private SettingsService _settingsService;
        public void Initialize()
        {
            Application.targetFrameRate = 60;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _windowsManager.ShowFastWindow<LoadingWindow>();
            await _settingsService.PlayMusicAsync(SoundKey.MenuMusic);
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellation);
            Vibration.Init();
            _windowsManager.ShowFastWindow<MainMenuWindow>();
            _windowsManager.HideWindow<LoadingWindow>();
            // await _gameManager.StartLevel(0);
        }
    }
}