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
        [SerializeField] private HintController _tutorialFirstBlockStage;
        [SerializeField] private GameObject _tutorialBalanceStage;
        [SerializeField] private GameObject _tutorialBoosterBalanceStage;

        private Color _currentBackLightBalanceColor;
        private int _panelBalanceSiblingIndex;
        private int _boosterBalanceButtonSiblingIndex;

        public override void Initialize()
        {
            base.Initialize();
            
            _tutorialFirstBlockStage.gameObject.SetActive(!_appData.User.IsTutorialFirstBlockPassed);
            
            _pauseMenuButton.onClick.AddListener(OpenPause);
            _settingsButton.onClick.AddListener(OpenSettings);
            _boosterBalanceButton.onClick.AddListener(UseBalanceBooster);
            _clickArea.onClick.AddListener(DropBlock);
            
            _appData.LevelData.LevelScoreReactive
                .Skip(1)
                .Subscribe(value =>
                    {
                        _textScore.text = Regex.Replace(_textScore.text, @"\d", "");
                        _textScore.text += value;
                    })
                .AddTo(Disposables);
            _appData.LevelData.HealthReactive
                .Skip(1)
                .Subscribe(value =>
                {
                    for (int i = 0; i < _healthImages.Count; i++)
                        _healthImages[i].sprite = i < value ? _imagesConfig.HeartFull : _imagesConfig.HeartEmpty;
                })
                .AddTo(Disposables);
            _appData.LevelData.TotalSwayImbalanceReactive
                .Skip(1)
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
        
        private void OpenPause()
        {
            _settingsService.PlaySfx(SoundKey.ButtonClick);
            WindowsManager.ShowWindow<PauseWindow>();
        }

        private void OpenSettings()
        {
            _settingsService.PlaySfx(SoundKey.ButtonClick);
            WindowsManager.ShowWindow<SettingsWindow>();
        }

        private void UseBalanceBooster()
        {
            _settingsService.PlaySfx(SoundKey.ButtonClick);
            _adsService.UseReward(_boosterService.ResetBalance);
        }

        private void DropBlock()
        {
            _settingsService.PlaySfx(SoundKey.ButtonClick);
            _blockDropService.DropBlock();
        }
        
        public void ShowTutorialBalance()
        {
            _panelBalanceSiblingIndex = _panelBalance.transform.GetSiblingIndex();

            _panelBalance.transform.SetAsLastSibling();
            _tutorialBalanceStage.SetActive(true);
        }
        
        public void ShowTutorialRewardButton()
        {
            _boosterBalanceButtonSiblingIndex = _boosterBalanceButton.transform.GetSiblingIndex();

            _boosterBalanceButton.transform.SetAsLastSibling();
            _tutorialBoosterBalanceStage.SetActive(true);
        }
        
        public void TutorialFirstBlockPassed()
        {
            _appData.User.IsTutorialFirstBlockPassed = true;
            _tutorialFirstBlockStage.Dispose();
            _tutorialFirstBlockStage.gameObject.SetActive(false);
            _clickArea.onClick.Invoke();
        }

        public void TutorialBalancePassed()
        {
            _appData.User.IsTutorialBalancePassed = true;
            _panelBalance.transform.SetSiblingIndex(_panelBalanceSiblingIndex);
            _tutorialBalanceStage.SetActive(false);
        }
        
        public void TutorialRewardButton()
        {
            _appData.User.IsTutorialBoosterBalancePassed = true;
            _boosterBalanceButton.transform.SetSiblingIndex(_boosterBalanceButtonSiblingIndex);
            _tutorialBoosterBalanceStage.SetActive(false);
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

        public override void Dispose()
        {
            base.Dispose();
            _pauseMenuButton.onClick.RemoveListener(OpenPause);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            _boosterBalanceButton.onClick.RemoveListener(UseBalanceBooster);
            _clickArea.onClick.RemoveListener(DropBlock);
        }
    }
}