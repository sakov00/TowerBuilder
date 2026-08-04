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
        private WindowsManager _windowsManager;
        private SettingsService _settingsService;
        private LanguageService _languageService;
        
        [Inject]
        public InitializeGame(WindowsManager windowsManager, SettingsService settingsService, LanguageService languageService)
        {
            _windowsManager = windowsManager;
            _settingsService = settingsService;
            _languageService = languageService;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = 60;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            Debug.Log(YG2.infoYG.Metrica.metricaCounterID);
            Vibration.Init();
            YG2.GetLanguage();
            _languageService.SetPlatformLanguage();
            _settingsService.PlayMusicAsync(SoundKey.MenuMusic).Forget();
            _windowsManager.ShowFastWindow<MainMenuWindow>();
            // await _gameManager.StartLevel(0);
        }
    }
}