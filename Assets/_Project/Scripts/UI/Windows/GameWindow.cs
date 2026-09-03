using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.ServicesGameplay;
using _Project.Scripts.SO;
using _Project.Scripts.UI.TweenFeature;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class GameWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SettingsService _settingsService;
        [Inject] private BlockDropService _blockDropService;
        [Inject] private BuildingConfig _buildingConfig;
        [Inject] private ImagesConfig _imagesConfig;
        [Inject] private AdsService _adsService;
        [Inject] private BoosterService _boosterService;

        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _boosterBalanceButton;
        [SerializeField] private Button _clickArea;
        [SerializeField] private RectTransform _panelBalance;
        [SerializeField] private RectTransform _arrowBalance;
        [SerializeField] private List<Image> _healthImages;
        [SerializeField] private TextMeshProUGUI _textFeedback;
        [SerializeField] private TextMeshProUGUI _textScore;
        [SerializeField] private Image _backLightBalance;
        
        [Header("Tutorials")]
        [SerializeField] private HintController _tutorialBlocksStage;
        [SerializeField] private GameObject _tutorialBalanceStage;

        private Color _currentBackLightBalanceColor;
        
        protected override void Awake()
        {
            base.Awake();
            
            _tutorialBlocksStage.gameObject.SetActive(!_appData.User.IsTutorialFirstBlockPassed);
            
            _pauseMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClick);
                    WindowsManager.ShowWindow<PauseWindow>();
                })
                .AddTo(Disposables);
            
            _settingsButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClick);
                    WindowsManager.ShowWindow<SettingsWindow>();
                })
                .AddTo(Disposables);
            
            _boosterBalanceButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClick);
                    _adsService.UseReward(_boosterService.ResetBalance);
                })
                .AddTo(Disposables);
            _clickArea.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClick);
                    _blockDropService.DropBlock();
                })
                .AddTo(Disposables);
            _appData.LevelData.LevelScoreReactive
                .Subscribe(value =>
                    {
                        _textScore.text = Regex.Replace(_textScore.text, @"\d", "");
                        _textScore.text += value;
                    })
                .AddTo(Disposables);
            _appData.LevelData.HealthReactive
                .Subscribe(value =>
                {
                    for (int i = 0; i < _healthImages.Count; i++)
                        _healthImages[i].sprite = i < value ? _imagesConfig.HeartFull : _imagesConfig.HeartEmpty;
                })
                .AddTo(Disposables);
            _appData.LevelData.TotalSwayImbalanceReactive
                .Subscribe(value =>
                {
                    float normalized = value / _buildingConfig.DestroyImbalance;
                    normalized = Mathf.Clamp(normalized, -1f, 1f);
                    float maxAngle = 90f;
                    float angle = normalized * maxAngle;
                    _arrowBalance.rotation = Quaternion.Euler(0f, 0f, -angle);
                })
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void TutorialBalancePassed()
        {
            _appData.User.IsTutorialBalancePassed = true;
        }

        public void Reset()
        {
            _backLightBalance.DOKill();
            _backLightBalance.color = new Color(1f, 0.92156863f, 0.015686275f, 0f);
            _panelBalance.anchoredPosition = new Vector2(0, -_panelBalance.rect.height);
            _healthImages.ForEach(x => x.sprite = _imagesConfig.HeartFull);
        }

        public void ShowText(string text, Color color)
        {
            _textFeedback.transform.localScale = Vector3.zero;
            _textFeedback.text = text;
            _textFeedback.color = color;
            
            var sequence = DOTween.Sequence();
            sequence.Append(_textFeedback.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack));
            sequence.AppendInterval(0.5f);
            sequence.Append(_textFeedback.transform.DOScale(0, 0.5f).SetEase(Ease.InBack));
            sequence.AppendCallback(() => _textFeedback.text = string.Empty);
        }
        
        public void ShowBalancePanel()
        {
            _panelBalance.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutBack);
        }

        public async UniTask SetBackLightBalance(bool show, Color color)
        {
            if(_currentBackLightBalanceColor == color)
                return;
            
            _currentBackLightBalanceColor = color;
            
            if (show)
            {
                await _backLightBalance.DOFade(0, 0.5f);
                _backLightBalance.DOKill();
                _backLightBalance
                    .DOFade(1, 0.5f)
                    .From(0)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .OnStepComplete(() =>
                    {
                        var current = _backLightBalance.color;
                        if (current.a <= 0.01f)
                        {
                            current.r = color.r;
                            current.g = color.g;
                            current.b = color.b;

                            _backLightBalance.color = current;
                        }
                    });
            }
            else
            {
                _backLightBalance.DOKill();
                _backLightBalance.DOFade(0, 0.5f);
            }
        }
    }
}