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
using YG;
using Object = UnityEngine.Object;

namespace _Project.Scripts
{
    public class InitializeGame : IInitializable, IAsyncStartable
    {
        private GameManager _gameManager;
        private SettingsService _settingsService;
        private LanguageService _languageService;

        [Inject]
        public InitializeGame(GameManager gameManager, SettingsService settingsService, LanguageService languageService)
        {
            _gameManager = gameManager;
            _settingsService = settingsService;
            _languageService = languageService;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = 60;
            Vibration.Init();
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, cancellation);
            await _gameManager.StartLevel(0, false);
            await UniTask.Yield(PlayerLoopTiming.Update, cancellation);
            _settingsService.PlayMusicAsync(SoundKey.MenuMusic).Forget();
            YG2.GameReadyAPI();
        }
    }
}