using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class PauseWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;
        [Inject] private SettingsService _settingsService;
        
        [Header("Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;

        public override void Initialize()
        {
            base.Initialize();
            _resumeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.HideWindow<PauseWindow>();
            }).AddTo(Disposables);
            
            _restartButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                await WindowsManager.ShowWindow<LoadingWindow>();
                WindowsManager.HideFastWindow<PauseWindow>();
                _gameManager.RestartLevel().Forget();
            }).AddTo(Disposables);
            
            _settingsButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                await WindowsManager.HideWindow<PauseWindow>();
                await WindowsManager.ShowWindow<SettingsWindow>();
            }).AddTo(Disposables);
            
            _mainMenuButton.OnClickAsObservable().Subscribe(async _ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                await WindowsManager.ShowWindow<LoadingWindow>();
                WindowsManager.HideFastWindow<GameWindow>();
                WindowsManager.HideFastWindow<PauseWindow>();
                WindowsManager.ShowFastWindow<MainMenuWindow>();
                WindowsManager.HideWindow<LoadingWindow>();
            }).AddTo(Disposables);
        }
    }
}