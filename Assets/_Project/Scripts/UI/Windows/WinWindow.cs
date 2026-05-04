using System;
using System.Linq;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.EnemyRoads;
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
            
            var spawns = _saveRegistry.GetAllByType<EnemyRoadController>();
            _isLevelCompleted = spawns.Any(spawn => spawn.CountRounds == _appData.LevelData.CurrentRound);
            if (_isLevelCompleted) 
                _appData.User.CurrentLevel++;
            else
                _appData.LevelData.CurrentRound++;
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
                await _gameManager.StartLevel(_appData.User.CurrentLevel);
            else
                await _gameManager.ResetRound();
            await WindowsManager.HideWindow<LoadingWindow>();
        }
    }
}