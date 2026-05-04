using System.Collections.Generic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.WindowElements;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class MainMenuWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SettingsService _settingsService;
        
        [SerializeField] private Button _currentLevelButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _settingsButton;
        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _crystalsText;

        [Header("Levels")]
        [SerializeField] private LevelScrollRect _scrollRect;
        [SerializeField] private List<LevelPanel> _levelPanels = new();
        [SerializeField] private int _totalLevels = 100;

        protected override void Awake()
        {
            base.Awake();
            _currentLevelButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                _scrollRect.ScrollToLevel(_appData.User.CurrentLevel);
            }).AddTo(Disposables);
            _upgradeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.ShowWindow<UpgradeWindow>();
            }).AddTo(Disposables);
            _settingsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.ShowWindow<SettingsWindow>();
            }).AddTo(Disposables);
            
            _appData.User.CurrentLevelReactive
                .Subscribe(levelIndex => _levelText.text = $"Level {levelIndex + 1}")
                .AddTo(Disposables);
            
            _appData.User.CrystalsReactive
                .Subscribe(crystalsCount => _crystalsText.text = $"{crystalsCount}")
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
            _scrollRect.Initialize(_appData.User.CurrentLevel, _levelPanels, _totalLevels);
        }
    }
}