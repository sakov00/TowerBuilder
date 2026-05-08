using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.ServicesGameplay;
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

        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private Button _clickArea;
        [SerializeField] private TextMeshProUGUI _textFeedback;
        [SerializeField] private TextMeshProUGUI _textScore;
        
        protected override void Awake()
        {
            base.Awake();
            
            _pauseMenuButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    WindowsManager.ShowWindow<PauseWindow>();
                })
                .AddTo(Disposables);
            _clickArea.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                    _blockDropService.DropBlock();
                })
                .AddTo(Disposables);
            _appData.LevelData.LevelScoreReactive
                .Subscribe(value => _textScore.text = $"Score: {value}")
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
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
    }
}