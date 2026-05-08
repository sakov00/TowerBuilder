using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class WinWindow : BaseWindow
    {
        [Inject] private SettingsService _settingsService;
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        [Inject] private SaveRegistry _saveRegistry;

        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        
        private bool _isLevelCompleted;

        protected override void Awake()
        {
            base.Awake();
            
            _homeButton.OnClickAsObservable()
                .Subscribe(_ => _settingsService.PlaySfx(SoundKey.ButtonClickSound))
                .AddTo(Disposables);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _settingsService.PlaySfx(SoundKey.ButtonClickSound))
                .AddTo(Disposables);

            _continueButton.OnClickAsObservable()
                .Subscribe(_ => _settingsService.PlaySfx(SoundKey.ButtonClickSound))
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        
        private void HomeOnClick()
        {
        }
        
        private async UniTaskVoid RestartOnClick()
        {
            await WindowsManager.HideWindow<WinWindow>();
            await _gameManager.RestartLevel();
        }
        
        private async UniTaskVoid ContinueOnClick()
        {
            await WindowsManager.ShowWindow<LoadingWindow>();
            WindowsManager.HideFastWindow<WinWindow>();
            if (_isLevelCompleted)
                await _gameManager.StartLevel(0);
            await WindowsManager.HideWindow<LoadingWindow>();
        }
    }
}