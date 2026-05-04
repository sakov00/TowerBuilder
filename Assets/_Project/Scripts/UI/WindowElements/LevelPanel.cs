using System;
using System.Collections.Generic;
using _Project.Scripts._VContainer;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.Windows;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.WindowElements
{
    public class LevelPanel : MonoBehaviour
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private GameManager _gameManager;
        [Inject] private SettingsService _settingsService;
        
        [SerializeField] private ReactiveProperty<LevelState> _currentState = new(LevelState.Locked);
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _roundText;
        
        [SerializeField] private Image _passedImageBg;
        [SerializeField] private Image _readyImageBg;
        [SerializeField] private Image _lockedImageBg;
        [SerializeField] private Image _lockedShadow;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _restartButton;
        
        [SerializeField] private Transform _enemyPanelsContainer;
        [SerializeField] private List<EnemyData> _enemies = new List<EnemyData>();
        
        private CompositeDisposable _disposables = new();
        
        private int _levelNumber;

        public LevelState CurrentState
        {
            get => _currentState.Value; 
            set => _currentState.Value = value;
        }

        private void Awake()
        {
            InjectManager.Inject(this);
            
            _currentState
                .Subscribe(state =>
                {
                    _passedImageBg.gameObject.SetActive(state == LevelState.Passed);
                    _readyImageBg.gameObject.SetActive(state == LevelState.Ready);
                    _lockedImageBg.gameObject.SetActive(state == LevelState.Locked);
                    _lockedShadow.gameObject.SetActive(state == LevelState.Locked);
                    _restartButton.gameObject.SetActive(state == LevelState.Passed);
                    _playButton.gameObject.SetActive(state == LevelState.Ready);
                })
                .AddTo(_disposables);

            _restartButton.onClick.AsObservable()
                .Where(_ => _currentState.Value == LevelState.Passed)
                .Subscribe(async _ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    await _windowsManager.ShowWindow<LoadingWindow>();
                    _windowsManager.HideFastWindow<MainMenuWindow>();
                    await _gameManager.StartLevel(_levelNumber);
                    _windowsManager.ShowFastWindow<GameWindow>();
                    _windowsManager.HideWindow<LoadingWindow>();
                })
                .AddTo(_disposables);
            
            _playButton.onClick.AsObservable()
                .Where(_ => _currentState.Value == LevelState.Ready)
                .Subscribe(async _ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    await _windowsManager.ShowWindow<LoadingWindow>();
                    _windowsManager.HideFastWindow<MainMenuWindow>();
                    await _gameManager.StartLevel(_levelNumber);
                    _windowsManager.ShowFastWindow<GameWindow>();
                    _windowsManager.HideWindow<LoadingWindow>();
                })
                .AddTo(_disposables);
        }

        public void Initialize(int levelNumber, int roundNumber, LevelState currentState)
        {
            _levelText.text = $"Level {levelNumber + 1}";
            _roundText.text = $"Round {roundNumber + 1}";
            CurrentState = currentState;
        }
        
        public void SetPosition(Vector2 position) => _rectTransform.anchoredPosition = position;

        public Vector2 GetPosition() => _rectTransform.anchoredPosition;
        
        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}