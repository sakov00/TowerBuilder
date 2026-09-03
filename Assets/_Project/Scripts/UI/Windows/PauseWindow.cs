using System.Text.RegularExpressions;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using YG;

namespace _Project.Scripts.UI.Windows
{
    public class PauseWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        [Inject] private SettingsService _settingsService;
        [Inject] private AdsService _adsService;
        [Inject] private AnalyticService _analyticService;
        
        [Header("Labels")]
        [SerializeField] private LanguageText _recordText;
        
        [Header("Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        public override void Initialize()
        {
            base.Initialize();
            
            _appData.User.ScoreRecordReactive
                .Subscribe(value =>
                {
                    _recordText.SetArguments(value);
                })
                .AddTo(Disposables);
            
            _resumeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                WindowsManager.HideWindow<PauseWindow>();
            }).AddTo(Disposables);
            
            _restartButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _analyticService.SendMessage("RestartClicked");
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                _adsService.UseInter(async () =>
                {
                    await WindowsManager.ShowWindow<LoadingWindow>();
                    WindowsManager.HideFastWindow<PauseWindow>();
                    _gameManager.RestartLevel().Forget();
                });
            }).AddTo(Disposables);
            
            _settingsButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                await WindowsManager.HideWindow<PauseWindow>();
                await WindowsManager.ShowWindow<SettingsWindow>();
            }).AddTo(Disposables);
            
            _mainMenuButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                await WindowsManager.ShowWindow<LoadingWindow>();
                WindowsManager.HideFastWindow<GameWindow>();
                WindowsManager.HideFastWindow<PauseWindow>();
                WindowsManager.ShowFastWindow<MainMenuWindow>();
                WindowsManager.HideWindow<LoadingWindow>();
            }).AddTo(Disposables);

            _appData.User.ScoreRecord = _appData.LevelData.LevelScore;
        }
    }
}