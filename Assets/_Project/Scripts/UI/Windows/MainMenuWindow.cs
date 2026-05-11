using System.Collections.Generic;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Services;
using _Project.Scripts.UI.WindowElements;
using Cysharp.Threading.Tasks;
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
        [Inject] private GameManager _gameManager;
        [Inject] private SettingsService _settingsService;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _crystalsText;

        protected override void Awake()
        {
            base.Awake();
            _playButton.OnClickAsObservable().Subscribe(_ =>
            {
                _gameManager.StartLevel(0).Forget();
                _settingsService.PlaySfx(SoundKey.ButtonClick);
            }).AddTo(Disposables);
            _upgradeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                // WindowsManager.ShowWindow<UpgradeWindow>();
            }).AddTo(Disposables);
            _settingsButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                WindowsManager.ShowWindow<SettingsWindow>();
            }).AddTo(Disposables);
            _exitButton.OnClickAsObservable().Subscribe(_ =>
            {
                _settingsService.PlaySfx(SoundKey.ButtonClick);
                Application.Quit();
            }).AddTo(Disposables);
            
            _appData.User.CrystalsReactive
                .Subscribe(crystalsCount => _crystalsText.text = $"{crystalsCount}")
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}