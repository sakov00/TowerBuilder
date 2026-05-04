using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.Scripts.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SettingsService _settingsService;
        
        [Header("UI Elements")]
        [SerializeField] private Button _musicButton;
        [SerializeField] private Image _musicImage;
        [SerializeField] private Sprite _musicOnSprite;
        [SerializeField] private Sprite _musicOffSprite;
        
        [SerializeField] private Button _soundButton;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Sprite _soundOnSprite;
        [SerializeField] private Sprite _soundOffSprite;
        
        [SerializeField] private Button _vibroButton;
        [SerializeField] private Image _vibroImage;
        [SerializeField] private Sprite _vibroOnSprite;
        [SerializeField] private Sprite _vibroOffSprite;
        
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _backButton;

        protected override void Awake()
        {
            base.Awake();

            _musicButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                SetMusicValue(!_appData.User.MusicIsActive);
            }).AddTo(Disposables);
            _soundButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                SetSoundValue(!_appData.User.SoundIsActive);
            }).AddTo(Disposables);
            _vibroButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                SetVibroValue(!_appData.User.VibroIsActive);
            }).AddTo(Disposables);
            
            _privacyButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.HideWindow<SettingsWindow>();
            }).AddTo(Disposables);
            _termsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.HideWindow<SettingsWindow>();
            }).AddTo(Disposables);
            _backButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClickSound);
                WindowsManager.HideWindow<SettingsWindow>();
            }).AddTo(Disposables);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            SetMusicValue(_appData.User.MusicIsActive);
            SetSoundValue(_appData.User.SoundIsActive);
            SetVibroValue(_appData.User.VibroIsActive);
        }

        private void SetMusicValue(bool value)
        {
            _musicImage.sprite = value ? _musicOnSprite : _musicOffSprite;
            _appData.User.MusicIsActive = value;
        }

        private void SetSoundValue(bool value)
        {
            _soundImage.sprite = value ? _soundOnSprite : _soundOffSprite;
            _appData.User.SoundIsActive = value;
        }

        private void SetVibroValue(bool value)
        {
            _vibroImage.sprite = value ? _vibroOnSprite : _vibroOffSprite;
            _appData.User.VibroIsActive = value;
        }
    }
}
