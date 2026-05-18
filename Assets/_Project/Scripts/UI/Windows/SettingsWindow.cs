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
    public class SettingsWindow : BaseWindow
    {
        [Inject] private AppData _appData;
        [Inject] private SettingsService _settingsService;
        
        [Header("UI Elements")]
        [SerializeField] private Button _musicButton;
        [SerializeField] private GameObject _musicCross;
        
        [SerializeField] private Button _soundButton;
        [SerializeField] private GameObject _soundCross;
        
        [SerializeField] private Button _vibroButton;
        [SerializeField] private GameObject _vibroCross;
        
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _backButton;

        protected override void Awake()
        {
            base.Awake();

            _musicButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                SetMusicValue(!_appData.User.MusicIsActive);
            }).AddTo(Disposables);
            _soundButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                SetSoundValue(!_appData.User.SoundIsActive);
            }).AddTo(Disposables);
            _vibroButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                SetVibroValue(!_appData.User.VibroIsActive);
            }).AddTo(Disposables);
            
            _privacyButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                Application.OpenURL("https://sakov00.github.io/Privacy-Policy-Terms-Of-Use/privacy-policy.html");
            }).AddTo(Disposables);
            _termsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                WindowsManager.HideWindow<SettingsWindow>();
                Application.OpenURL("https://sakov00.github.io/Privacy-Policy-Terms-Of-Use/terms-of-use.html");
            }).AddTo(Disposables);
            _backButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
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
            _musicCross.SetActive(!value);
            _appData.User.MusicIsActive = value;
            if (value)
                _settingsService.PlayMusicAsync(SoundKey.MenuMusic).Forget();
            else
                _settingsService.StopMusic();
        }

        private void SetSoundValue(bool value)
        {
            _soundCross.SetActive(!value);
            _appData.User.SoundIsActive = value;
        }

        private void SetVibroValue(bool value)
        {
            _vibroCross.SetActive(!value);
            _appData.User.VibroIsActive = value;
        }
    }
}
