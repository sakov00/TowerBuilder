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
    public class FailWindow : BaseWindow
    {
        [Inject] private SettingsService _settingsService;
        [Inject] private AppData _appData;
        [Inject] private GameManager _gameManager;

        [Header("Buttons")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _restartButton;

        protected override void Awake()
        {
            base.Awake();
            
            _homeButton.OnClickAsObservable()
                .Subscribe(_ => _settingsService.PlaySfx(SoundKey.ButtonClickSound))
                .AddTo(Disposables);

            _restartButton.OnClickAsObservable()
                .Subscribe(_ => _settingsService.PlaySfx(SoundKey.ButtonClickSound))
                .AddTo(Disposables);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        
        private void HomeOnClick()
        {
        }
        
        private async UniTaskVoid RestartOnClick()
        {
            await WindowsManager.HideWindow<FailWindow>();
            await _gameManager.RestartLevel();
        }
    }
}